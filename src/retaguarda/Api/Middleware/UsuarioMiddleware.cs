using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Retaguarda.Servicos;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.Api.Middleware
{
    public class UsuarioMiddleware
    {
        private readonly RequestDelegate _next;

        public UsuarioMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // If user is authenticated, try to load domain Usuario and set in RequisicaoUsuario
            if (context.User?.Identity?.IsAuthenticated == true)
            {
                var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ?? context.User.FindFirst(JwtRegisteredClaimNames.Sub);
                if (idClaim != null && long.TryParse(idClaim.Value, out var userId))
                {
                    var usuarioSvc = context.RequestServices.GetService(typeof(IUsuarioServico)) as IUsuarioServico;
                    var reqUsuario = context.RequestServices.GetService(typeof(RequisicaoUsuario)) as RequisicaoUsuario;
                    if (usuarioSvc != null && reqUsuario != null)
                    {
                        var u = await usuarioSvc.ObterPorIdAsync(userId);
                        reqUsuario.Usuario = u;
                    }
                }
            }

            await _next(context);
        }
    }
}
