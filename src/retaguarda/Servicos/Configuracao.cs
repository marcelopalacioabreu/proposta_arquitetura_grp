using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Retaguarda.Servicos
{
    public static class Configuracao
    {
        /// <summary>
        /// Registra serviços de domínio e auxiliares para injeção de dependência.
        /// </summary>
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            services.AddScoped<Interfaces.IOrganizacaoServico, OrganizacaoServico>();
            services.AddScoped<Interfaces.IUsuarioServico, UsuarioServico>();
            services.AddScoped<Interfaces.IPermissionService, PermissionService>();
            services.AddScoped<RequisicaoUsuario>();
            services.AddScoped<EscopoEmExecucao>();
            return services;
        }
    }
}
