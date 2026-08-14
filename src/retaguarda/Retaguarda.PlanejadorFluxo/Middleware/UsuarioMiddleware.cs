using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Retaguarda.Servicos;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.PlanejadorFluxo.Middleware
{
    /// <summary>
    /// Middleware que carrega dados do usuário autenticado.
    /// 
    /// Procura pelo ClaimTypes.NameIdentifier no token JWT
    /// e preenche RequisicaoUsuario com dados da entidade Usuario do banco.
    /// </summary>
    public class UsuarioMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UsuarioMiddleware> _logger;

        public UsuarioMiddleware(RequestDelegate next, ILogger<UsuarioMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Se usuário está autenticado, tenta carregar dados da entidade Usuario do banco
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) 
                    ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub);
                
                if (idClaim != null && long.TryParse(idClaim.Value, out var userId))
                {
                    var usuarioSvc = context.RequestServices.GetService(typeof(IUsuarioServico)) as IUsuarioServico;
                    var reqUsuario = context.RequestServices.GetService(typeof(RequisicaoUsuario)) as RequisicaoUsuario;
                    
                    if (usuarioSvc != null && reqUsuario != null)
                    {
                        try
                        {
                            var u = await usuarioSvc.ObterPorIdAsync(userId);
                            reqUsuario.Usuario = u;
                            
                            _logger.LogDebug("UsuarioMiddleware: Usuário carregado {UserId} ({UserName})", 
                                userId, u?.Nome ?? "desconhecido");
                        }
                        catch (Exception ex)
                        {
                            // Falha silenciosa: não interrompe a requisição
                            _logger.LogWarning("UsuarioMiddleware: Erro ao carregar usuário {UserId}: {Message}", 
                                userId, ex.Message);
                        }
                    }
                }
            }

            await _next(context);
        }
    }
}
