using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Elsa.Extensions;
using Elsa.Persistence.EFCore.Extensions;
using Elsa.Persistence.EFCore.Modules.Management;
using Elsa.Persistence.EFCore.Modules.Runtime;
using Retaguarda.Servicos;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseStaticWebAssets();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

// Share DataProtection keys with the main API
var keysPath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "data-protection-keys"));
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(keysPath))
    .SetApplicationName("Retaguarda");

// Expose the main app DB, repositories and services so custom Elsa activities can inject them
builder.Services.AddHttpContextAccessor();
Retaguarda.Persistencia.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Repositorios.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Servicos.Configuracao.RegistrarServices(builder.Services, builder.Configuration);

var services = builder.Services;
var configuration = builder.Configuration;

var connectionString = configuration.GetSection("Elsa:ConnectionStrings")
    .GetValue<string>("DefaultConnection")
    ?? configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No database connection string configured.");

var jwtKey = configuration["Jwt:Key"] ?? "change_this_secret_for_prod";

// Setup JWT Bearer authentication
var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey);
if (keyBytes.Length < 32)
{
    using var sha = System.Security.Cryptography.SHA256.Create();
    keyBytes = sha.ComputeHash(keyBytes);
}

services.AddAuthentication(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes),
        ValidateIssuer = true,
        ValidIssuer = "Retaguarda",
        ValidateAudience = false,
        ValidateLifetime = false
    };
    // Fallback: read JWT from HttpOnly cookie when no Authorization header is present
    options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (string.IsNullOrEmpty(context.Request.Headers["Authorization"].ToString())
                && context.Request.Cookies.TryGetValue("access_token", out var cookieToken))
            {
                context.Token = cookieToken;
            }
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

services.AddAuthorization();

// Registrar EscopoEmExecucao e RequisicaoUsuario como Scoped
// Necessário para que AtuacaoMiddleware possa preencher contexto de tenant
services.AddScoped<EscopoEmExecucao>();
services.AddScoped<Retaguarda.Servicos.RequisicaoUsuario>();

services.AddElsa(elsa => elsa
    .UseWorkflowManagement(management => management.UseEntityFrameworkCore(ef =>
    {
        ef.UsePostgreSql(connectionString);
        ef.RunMigrations = builder.Environment.IsDevelopment();
    }))
    .UseWorkflowRuntime(runtime => runtime.UseEntityFrameworkCore(ef =>
    {
        ef.UsePostgreSql(connectionString);
        ef.RunMigrations = builder.Environment.IsDevelopment();
    }))
    .UseScheduling()
    .UseJavaScript()
    .UseLiquid()
    .UseCSharp()
    .UseHttp(http => http.ConfigureHttpOptions = options => configuration.GetSection("Http").Bind(options))
    .UseWorkflowsApi()
    .AddActivitiesFrom<Program>()
    .AddWorkflowsFrom<Program>()
);

services.AddCors(cors => cors.AddDefaultPolicy(policy =>
    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().WithExposedHeaders("*")));

// Required for [ApiController] + [Authorize] to work correctly in .NET 9
services.AddControllers();

services.AddRazorPages(options =>
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseRouting();

// Map simple endpoints that don't require authentication
app.MapGet("/health", () => Results.Ok("OK"));

app.UseCors();
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

app.UseAuthentication();

// Middlewares para resolver contexto multilocatário (tenant)
// ORDEM CRÍTICA: Executar ANTES de UseAuthorization() e UseWorkflowsApi()
// 1. UsuarioMiddleware: carrega dados da entidade Usuario do banco
// 2. AtuacaoMiddleware: extrai OrganizacaoId/UnidadeId/SetorId de cookie ou header X-Atuacao
// 3. Popula HttpContext.Items e EscopoEmExecucao para uso nas atividades Elsa
app.UseMiddleware<Retaguarda.PlanejadorFluxo.Middleware.UsuarioMiddleware>();
app.UseMiddleware<Retaguarda.PlanejadorFluxo.Middleware.AtuacaoMiddleware>();

app.UseAuthorization();

// Middleware para filtrar workflows Elsa por tenant (OrganizacaoId)
app.UseMiddleware<Retaguarda.PlanejadorFluxo.Middleware.ElsaTenantFilterMiddleware>();

app.UseWorkflowsApi();
app.UseWorkflows();

app.MapControllers();

// Identity token endpoint (must be after routing but before any catch-alls)

// Reads the HttpOnly access_token cookie server-side and returns the validated JWT + user info.
// Called by CookieTokenHandler and CookieAuthStateProvider in Elsa Studio WASM.
// NOT protected by [Authorize] — it validates the cookie internally.
app.MapGet("/identity/token", (HttpContext ctx, IConfiguration cfg) =>
{
    if (!ctx.Request.Cookies.TryGetValue("access_token", out var token))
        return Results.Unauthorized();

    try
    {
        var rawKey = cfg["Jwt:Key"] ?? "change_this_secret_for_prod";
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(rawKey);
        if (keyBytes.Length < 32)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            keyBytes = sha.ComputeHash(keyBytes);
        }
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token,
            new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes),
                ValidateIssuer   = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.FromMinutes(1)
            }, out _);

        var name = principal.FindFirst("name")?.Value
            ?? principal.Identity?.Name
            ?? "Usuário";

        return Results.Ok(new { token, name });
    }
    catch { return Results.Unauthorized(); }
});

// Prevent HTML host page from being returned for unmatched /elsa paths (would break JSON parsing in Blazor)
app.MapFallback("/elsa/{**path}", () => Results.NotFound(new { title = "Not found", status = 404 }));
app.MapFallbackToPage("/_Host");

app.Run("http://0.0.0.0:6001");
