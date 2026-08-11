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

// Persiste as chaves de proteção de dados em uma pasta de nível de espaço de trabalho para que outros aplicativos (por exemplo, PlanejadorFluxo) possam compartilhar o mesmo anel de chaves e validar cookies/tickets quando necessário.
// Pode ser necessário ajustar as permissões de leitura/gravação para a pasta de chaves, dependendo do ambiente de hospedagem (por exemplo, IIS, Docker, etc.).
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "..", "..", "data-protection-keys"))))
    .SetApplicationName("Retaguarda");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Permite selecionar o provedor EF em tempo de execução via configuração:
// Persistence:Provider = "Postgres" (padrão) ou "MySql"
var persistenceProvider = builder.Configuration["Persistence:Provider"] ?? "Postgres";

// Contexto de atuação (organizacao/unidade/setor) com escopo de requisição, preenchido pelo middleware
// `AddHttpContextAccessor` é necessário para `ApplicationDbContext` e outros helpers.
builder.Services.AddHttpContextAccessor();

// Registro centralizado: persistência (DbContext), repositórios e serviços de domínio
Retaguarda.Persistencia.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Repositorios.Configuracao.RegistrarServices(builder.Services, builder.Configuration);
Retaguarda.Servicos.Configuracao.RegistrarServices(builder.Services, builder.Configuration);

// JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "change_this_secret_for_prod";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Retaguarda";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);

// Garante que a chave seja de pelo menos 256 bits (32 bytes) para HS256. Se for mais curta, use SHA256 da chave fornecida.
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

        // Permite que o token seja enviado via cookie chamado "access_token" quando não estiver presente no cabeçalho Authorization
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

// Registra políticas de autorização para permissões declaradas em metadados (modulos.json)
builder.Services.AddAuthorization(options =>
{
    try
    {
        // Procura por modulos.json nos metadados do projeto ou faz fallback para documentação
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
                                    //Adiciona política que usa PermissionRequirement (verificado via DB no handler)
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
        // Ignore erros durante o registro de políticas para evitar quebrar a inicialização
    }
});

// Registra o manipulador de autorização que verifica permissões no banco de dados
builder.Services.AddScoped<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, Retaguarda.Api.Authorization.PermissionAuthorizationHandler>();
// O serviço de permissões é registrado por Retaguarda.Servicos.Configuracao

builder.Services.AddControllers(options =>
{
    // Registra um filtro de ação global que encapsula os resultados em EnvelopeResult
    options.Filters.Add<EnvelopeActionFilter>();
}).AddJsonOptions(opts =>
{
    // Evita erros quando o EF Core cria grafos de objetos com referências de volta
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    // Manter a profundidade máxima padrão (32) a menos que surjam necessidades explícitas
});
var app = builder.Build();

app.UseAuthentication();
// Garante que as informações do usuário sejam carregadas em RequisicaoUsuario antes do AtuacaoMiddleware
app.UseMiddleware<Retaguarda.Api.Middleware.UsuarioMiddleware>();
app.UseMiddleware<Retaguarda.Api.Middleware.AtuacaoMiddleware>();
app.UseAuthorization();

// Inicializa o banco de dados com o usuário administrador padrão se não existir (conveniência de desenvolvimento)
Retaguarda.Persistencia.Inicializadores.SeedData.EnsureSeed(app.Services);

app.MapControllers();

app.Run();
