using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Repositorios;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Servicos;
using Retaguarda.Api.Filters;
using Retaguarda.Api.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Persist DataProtection keys in a workspace-level folder so other apps (e.g. PlanejadorFluxo)
// can share the same key ring and validate cookies/tickets when necessary.
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "data-protection-keys"))))
    .SetApplicationName("Retaguarda");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Allow selecting the EF provider at runtime via configuration:
// Persistence:Provider = "Postgres" (default) or "MySql"
var persistenceProvider = builder.Configuration["Persistence:Provider"] ?? "Postgres";
// Register persistence, repositories and domain services via centralized configuration helpers
// These helpers will register the correct DbContext (Postgres/MySQL) and repository/service implementations.
Retaguarda.Persistencia.Configuracao.RegistrarServices(builder.Services, builder.Configuration);

// Request-scoped atuacao context (organizacao/unidade/setor) populated by middleware
// `AddHttpContextAccessor` is required by `ApplicationDbContext` and other helpers.
builder.Services.AddHttpContextAccessor();

// Centralized registration: persistence (DbContext), repositories and domain services
Retaguarda.Persistencia.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Repositorios.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Servicos.Configuracao.RegistrarServices(builder.Services, builder.Configuration);

// JWT settings
var jwtKey = builder.Configuration["Jwt:Key"] ?? "change_this_secret_for_prod";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Retaguarda";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
// Ensure key is at least 256 bits (32 bytes) for HS256. If shorter, use SHA256 of the provided key.
if (keyBytes.Length < 32)
{
    using var sha = SHA256.Create();
    keyBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(jwtKey));
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
        };

        // Allow token from cookie named "access_token" when not present in Authorization header
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) && context.Request.Cookies.ContainsKey("access_token"))
                {
                    context.Token = context.Request.Cookies["access_token"];
                }
                return Task.CompletedTask;
            }
        };
    });

// Register authorization policies for permissions declared in metadata (modulos.json)
builder.Services.AddAuthorization(options =>
{
    try
    {
        // Look for modulos.json in project metadata or documentation fallback
        var projectMeta = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "Metadados", "Contratos", "Modulos", "modulos.json"));
        var docMeta = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "..", "DOCUMENTACAO", "METADADOS", "Modulos", "modulos.json"));
        string? metaPath = null;
        if (System.IO.File.Exists(projectMeta)) metaPath = projectMeta;
        else if (System.IO.File.Exists(docMeta)) metaPath = docMeta;

        if (!string.IsNullOrEmpty(metaPath))
        {
            var txt = System.IO.File.ReadAllText(metaPath);
            using var doc = System.Text.Json.JsonDocument.Parse(txt);
            if (doc.RootElement.TryGetProperty("modulos", out var mods) && mods.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var g in mods.EnumerateArray())
                {
                    if (!g.TryGetProperty("itens", out var items) || items.ValueKind != System.Text.Json.JsonValueKind.Array) continue;
                    foreach (var it in items.EnumerateArray())
                    {
                        if (!it.TryGetProperty("permissoes", out var perms) || perms.ValueKind != System.Text.Json.JsonValueKind.Array) continue;
                        foreach (var p in perms.EnumerateArray())
                        {
                            if (p.ValueKind != System.Text.Json.JsonValueKind.Object) continue;
                            if (p.TryGetProperty("id", out var idProp) && idProp.ValueKind == System.Text.Json.JsonValueKind.String)
                            {
                                var pid = idProp.GetString();
                                if (!string.IsNullOrEmpty(pid))
                                {
                                    // Add policy that uses PermissionRequirement (checked via DB in handler)
                                    options.AddPolicy(pid, policy => policy.Requirements.Add(new PermissionRequirement(pid)));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    catch
    {
        // ignore errors during policy registration to avoid breaking startup
    }
});

// Register authorization handler that checks permissions in database
builder.Services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Retaguarda.Api.Authorization.PermissionAuthorizationHandler>();
// Permission service is registered by Retaguarda.Servicos.Configuracao

builder.Services.AddControllers(options =>
{
    // Register a global action filter that wraps results into EnvelopeResult
    options.Filters.Add<EnvelopeActionFilter>();
}).AddJsonOptions(opts =>
{
    // Avoid errors when EF Core creates object graphs with back-references
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // keep default max depth (32) unless explicit needs arise
});
var app = builder.Build();

app.UseAuthentication();
// Ensure user info is loaded into RequisicaoUsuario before AtuacaoMiddleware
app.UseMiddleware<Retaguarda.Api.Middleware.UsuarioMiddleware>();
app.UseMiddleware<Retaguarda.Api.Middleware.AtuacaoMiddleware>();
app.UseAuthorization();

// Seed default admin user if missing (development convenience)
Retaguarda.Persistencia.Inicializadores.SeedData.EnsureSeed(app.Services);

app.MapControllers();

app.Run();
