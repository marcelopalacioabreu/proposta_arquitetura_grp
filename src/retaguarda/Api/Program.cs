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
using Retaguarda.DTO.Converters;
using Retaguarda.Api.Binders;

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

// HttpClient para chamadas entre microsserviços
builder.Services.AddHttpClient();

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
    
    // Registra custom model binder para PesquisaParametrosDto
    // Extrai TODOS os query parameters e coloca no dicionário Filtros
    options.ModelBinderProviders.Insert(0, new PesquisaParametrosDtoBinderProvider());
}).AddJsonOptions(opts =>
{
    // Evita erros quando o EF Core cria grafos de objetos com referências de volta
    opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    
    // Registra conversores flexíveis para DateTime
    // Aceita múltiplos formatos: ISO 8601, HTML5 datetime-local, formato brasileiro
    opts.JsonSerializerOptions.Converters.Add(new FlexibleDateTimeConverter());
    opts.JsonSerializerOptions.Converters.Add(new FlexibleNullableDateTimeConverter());
    
    // Manter a profundidade máxima padrão (32) a menos que surjam necessidades explícitas
});
var app = builder.Build();

app.UseAuthentication();
// Garante que as informações do usuário sejam carregadas em RequisicaoUsuario antes do AtuacaoMiddleware
app.UseMiddleware<Retaguarda.Api.Middleware.UsuarioMiddleware>();
app.UseMiddleware<Retaguarda.Api.Middleware.AtuacaoMiddleware>();
app.UseAuthorization();

// Reverse proxy para Elsa/PlanejadorFluxo (porta 6001)
// O frontend envia requisições para /elsa/* e elas são roteadas para o PlanejadorFluxo
// com os cookies intactos (mesmo domínio: localhost:5000)
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/elsa", out var remainingPath))
    {
        var targetUrl = $"http://localhost:6001/elsa{remainingPath}{context.Request.QueryString}";
        
        using var httpClient = new HttpClient();
        
        // Copia o método e headers da requisição original
        var targetRequest = new HttpRequestMessage(
            new HttpMethod(context.Request.Method),
            targetUrl
        );
        
        // Headers hop-by-hop não devem ser repassados ao backend
        var hopByHop = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Host", "Connection", "Keep-Alive", "Transfer-Encoding",
            "TE", "Trailers", "Upgrade", "Proxy-Authorization", "Proxy-Authenticate"
        };

        foreach (var header in context.Request.Headers)
        {
            if (!hopByHop.Contains(header.Key))
            {
                targetRequest.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }
        
        // Adicionar header X-Atuacao com contexto multilocatário (CORREÇÃO 2)
        // Permite que PlanejadorFluxo saiba qual organização está acessando os workflows
        var escopo = context.RequestServices.GetService(typeof(EscopoEmExecucao)) as EscopoEmExecucao;
        if (escopo?.OrganizacaoId.HasValue == true)
        {
            var atuacao = new
            {
                organizacaoId = escopo.OrganizacaoId,
                organizacaoUnidadeId = escopo.OrganizacaoUnidadeId,
                setorId = escopo.SetorId
            };
            var atuacaoJson = System.Text.Json.JsonSerializer.Serialize(atuacao);
            targetRequest.Headers.Add("X-Atuacao", atuacaoJson);
        }
        
        // Copia o corpo se existir
        if (context.Request.Method != "GET" && context.Request.Method != "HEAD")
        {
            targetRequest.Content = new StreamContent(context.Request.Body);
            if (context.Request.ContentType != null)
            {
                targetRequest.Content.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse(context.Request.ContentType);
            }
        }
        
        try
        {
            var response = await httpClient.SendAsync(targetRequest, HttpCompletionOption.ResponseHeadersRead);
            
            // Copia status code
            context.Response.StatusCode = (int)response.StatusCode;
            
            // Copia headers da resposta, ignorando hop-by-hop
            var hopByHopResponse = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Connection", "Keep-Alive", "Transfer-Encoding",
                "TE", "Trailers", "Upgrade", "Proxy-Authorization", "Proxy-Authenticate"
            };
            foreach (var header in response.Headers)
            {
                if (!hopByHopResponse.Contains(header.Key))
                    context.Response.Headers.TryAdd(header.Key, header.Value.ToArray());
            }
            
            if (response.Content.Headers.ContentType != null)
            {
                context.Response.Headers.TryAdd("Content-Type", response.Content.Headers.ContentType.ToString());
            }
            
            // Copia corpo da resposta
            using var contentStream = await response.Content.ReadAsStreamAsync();
            await contentStream.CopyToAsync(context.Response.Body);
            
            return;
        }
        catch (Exception ex)
        {
            // Só modifica a resposta se os headers ainda não foram enviados
            if (!context.Response.HasStarted)
            {
                context.Response.StatusCode = 502;
                await context.Response.WriteAsJsonAsync(new { error = "Bad Gateway", message = ex.Message });
            }
            return;
        }
    }
    
    await next(context);
});

// Inicializa o banco de dados com o usuário administrador padrão se não existir (conveniência de desenvolvimento)
Retaguarda.Persistencia.Inicializadores.SeedData.EnsureSeed(app.Services);

app.MapControllers();

// Endpoint para ElsaStudio: lê o HttpOnly cookie access_token e retorna o JWT + informações do usuário
// Chamado por CookieTokenHandler e CookieAuthStateProvider no Blazor WASM do ElsaStudio
// NÃO protegido por [Authorize] — valida o cookie internamente
app.MapGet("/identity/token", (HttpContext ctx, IConfiguration cfg) =>
{
    if (!ctx.Request.Cookies.TryGetValue("access_token", out var token))
        return Results.Unauthorized();

    try
    {
        var rawKey = cfg["Jwt:Key"] ?? "change_this_secret_for_prod";
        var keyBytes = Encoding.UTF8.GetBytes(rawKey);
        if (keyBytes.Length < 32)
        {
            using var sha = SHA256.Create();
            keyBytes = sha.ComputeHash(keyBytes);
        }
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token,
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            }, out _);

        var name = principal.FindFirst("name")?.Value
            ?? principal.Identity?.Name
            ?? "Usuário";

        return Results.Ok(new { token, name });
    }
    catch { return Results.Unauthorized(); }
});

app.Run("http://0.0.0.0:5000");
