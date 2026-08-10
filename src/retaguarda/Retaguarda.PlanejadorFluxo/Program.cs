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

services.AddElsa(elsa => elsa
    .UseIdentity(identity =>
    {
        identity.TokenOptions = options => options.SigningKey = jwtKey;
        identity.UseAdminUserProvider();
    })
    .UseDefaultAuthentication()
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

services.AddRazorPages(options =>
    options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));

// Accept the main-app HttpOnly cookie as a Bearer token so same-origin Elsa API calls are authenticated
services.PostConfigure<Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerOptions>(
    Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme,
    opts =>
    {
        var prev = opts.Events?.OnMessageReceived;
        opts.Events ??= new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents();
        opts.Events.OnMessageReceived = async ctx =>
        {
            if (prev != null) await prev(ctx);
            if (string.IsNullOrEmpty(ctx.Token) && ctx.Request.Cookies.ContainsKey("access_token"))
                ctx.Token = ctx.Request.Cookies["access_token"];
        };
    });

// Named client retained for ProxyController (/planejadorDeFluxo/{**path})
services.AddHttpClient("Elsa", client =>
{
    var baseUrl = configuration["Elsa:BaseUrl"] ?? "http://localhost:6001";
    client.BaseAddress = new Uri(baseUrl);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Catch exceptions from Elsa middleware and return JSON so Blazor never receives HTML
app.Use(async (ctx, next) =>
{
    if (!ctx.Request.Path.StartsWithSegments("/elsa"))
    {
        await next(ctx);
        return;
    }
    try
    {
        await next(ctx);
    }
    catch (Exception ex)
    {
        if (!ctx.Response.HasStarted)
        {
            ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsJsonAsync(new
            {
                title = "An error occurred processing the request",
                status = 500,
                detail = app.Environment.IsDevelopment() ? ex.Message : null
            });
        }
    }
});

app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseWorkflowsApi();
app.UseWorkflows();
app.MapRazorPages();
app.MapControllers();
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

app.Run();
