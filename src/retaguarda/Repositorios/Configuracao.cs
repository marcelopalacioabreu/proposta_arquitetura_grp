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
            services.AddScoped<Interfaces.IOrquestracaoFluxoProcessoRepositorio, OrquestracaoFluxoProcessoRepositorio>();
            services.AddScoped<Interfaces.IPaisRepositorio, PaisRepositorio>();
            services.AddScoped<Interfaces.IUfRepositorio, UfRepositorio>();
            services.AddScoped<Interfaces.IMunicipioRepositorio, MunicipioRepositorio>();
            services.AddScoped<Interfaces.IBairroRepositorio, BairroRepositorio>();
            services.AddScoped<Interfaces.ITipoUnidadeRepositorio, TipoUnidadeRepositorio>();
            services.AddScoped<Interfaces.IPerfilRepositorio, PerfilRepositorio>();
            services.AddScoped<Interfaces.IDocumentoTipoRepositorio, DocumentoTipoRepositorio>();
            services.AddScoped<Interfaces.ITipoContatoRepositorio, TipoContatoRepositorio>();
            services.AddScoped<Interfaces.ITipoEnderecoRepositorio, TipoEnderecoRepositorio>();
            services.AddScoped<Interfaces.ITipoImovelRepositorio, TipoImovelRepositorio>();
            services.AddScoped<Interfaces.INaturezaJuridicaRepositorio, NaturezaJuridicaRepositorio>();
            services.AddScoped<Interfaces.INivelGovernoRepositorio, NivelGovernoRepositorio>();
            services.AddScoped<Interfaces.ISituacaoRepositorio, SituacaoRepositorio>();
            services.AddScoped<Interfaces.ICepRepositorio, CepRepositorio>();
            services.AddScoped<Interfaces.ILogradouroRepositorio, LogradouroRepositorio>();
            services.AddScoped<Interfaces.IImovelRepositorio, ImovelRepositorio>();
            services.AddScoped<Interfaces.IPessoaRepositorio, PessoaRepositorio>();
            services.AddScoped<Interfaces.IEnderecoRepositorio, EnderecoRepositorio>();
            services.AddScoped<Interfaces.IOrganizacaoUnidadeRepositorio, OrganizacaoUnidadeRepositorio>();
            services.AddScoped<Interfaces.IOrganizacaoUnidadeSetorRepositorio, OrganizacaoUnidadeSetorRepositorio>();
            return services;
        }
    }
}
