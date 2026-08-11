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
            services.AddScoped<Interfaces.IOrquestracaoFluxoProcessoServico, OrquestracaoFluxoProcessoServico>();
            services.AddScoped<Interfaces.IPaisServico, PaisServico>();
            services.AddScoped<Interfaces.IUfServico, UfServico>();
            services.AddScoped<Interfaces.IMunicipioServico, MunicipioServico>();
            services.AddScoped<Interfaces.IBairroServico, BairroServico>();
            services.AddScoped<Interfaces.ITipoUnidadeServico, TipoUnidadeServico>();
            services.AddScoped<Interfaces.IPerfilServico, PerfilServico>();
            services.AddScoped<Interfaces.IDocumentoTipoServico, DocumentoTipoServico>();
            services.AddScoped<Interfaces.ITipoContatoServico, TipoContatoServico>();
            services.AddScoped<Interfaces.ITipoEnderecoServico, TipoEnderecoServico>();
            services.AddScoped<Interfaces.ITipoImovelServico, TipoImovelServico>();
            services.AddScoped<Interfaces.INaturezaJuridicaServico, NaturezaJuridicaServico>();
            services.AddScoped<Interfaces.INivelGovernoServico, NivelGovernoServico>();
            services.AddScoped<Interfaces.ISituacaoServico, SituacaoServico>();
            services.AddScoped<Interfaces.ICepServico, CepServico>();
            services.AddScoped<Interfaces.ILogradouroServico, LogradouroServico>();
            services.AddScoped<Interfaces.IImovelServico, ImovelServico>();
            services.AddScoped<Interfaces.IPessoaServico, PessoaServico>();
            services.AddScoped<Interfaces.IEnderecoServico, EnderecoServico>();
            services.AddScoped<Interfaces.IOrganizacaoUnidadeServico, OrganizacaoUnidadeServico>();
            services.AddScoped<Interfaces.IOrganizacaoUnidadeSetorServico, OrganizacaoUnidadeSetorServico>();
            services.AddScoped<RequisicaoUsuario>();
            services.AddScoped<EscopoEmExecucao>();
            return services;
        }
    }
}
