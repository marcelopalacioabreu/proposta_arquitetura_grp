using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Retaguarda.Repositorios
{
    public static class Configuracao
    {
        /// <summary>
        /// Registra repositórios para injeção de dependência.
        /// </summary>
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            services.AddScoped<Interfaces.IOrganizacaoRepositorio, OrganizacaoRepositorio>();
            services.AddScoped<Interfaces.IUsuarioRepositorio, UsuarioRepositorio>();
            return services;
        }
    }
}
