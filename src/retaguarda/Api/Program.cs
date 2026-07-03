using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Repositorios;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Servicos;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// DI: repositories and services
builder.Services.AddScoped<IOrganizacaoRepositorio, OrganizacaoRepositorio>();
builder.Services.AddScoped<IOrganizacaoServico, OrganizacaoServico>();
builder.Services.AddScoped<Retaguarda.Repositorios.Interfaces.IUsuarioRepositorio, Retaguarda.Repositorios.UsuarioRepositorio>();
builder.Services.AddScoped<Retaguarda.Servicos.Interfaces.IUsuarioServico, Retaguarda.Servicos.UsuarioServico>();
builder.Services.AddScoped<Retaguarda.Servicos.RequisicaoUsuario>();

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

builder.Services.AddAuthorization();

builder.Services.AddControllers().AddJsonOptions(opts =>
{
    // Avoid errors when EF Core creates object graphs with back-references
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // keep default max depth (32) unless explicit needs arise
});
var app = builder.Build();

app.UseAuthentication();
app.UseMiddleware<Retaguarda.Api.Middleware.UsuarioMiddleware>();
app.UseAuthorization();

// Seed default admin user if missing (development convenience)
Retaguarda.Persistencia.Inicializadores.SeedData.EnsureSeed(app.Services);

app.MapControllers();

app.Run();
