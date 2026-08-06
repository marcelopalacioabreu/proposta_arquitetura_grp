using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia;

namespace Retaguarda.Api.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly Retaguarda.Servicos.Interfaces.IPermissionService _permissionService;

        public PermissionAuthorizationHandler(Retaguarda.Servicos.Interfaces.IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true) return;

            var idClaim = context.User.FindFirst(ClaimTypes.NameIdentifier) ?? context.User.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub);
            if (idClaim == null) return;
            if (!long.TryParse(idClaim.Value, out var userId)) return;

            var isAdmin = await _permissionService.IsUserAdministratorAsync(userId);
            if (isAdmin)
            {
                context.Succeed(requirement);
                return;
            }

            var perms = await _permissionService.GetPermissionsForUserAsync(userId);
            if (perms != null && perms.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
