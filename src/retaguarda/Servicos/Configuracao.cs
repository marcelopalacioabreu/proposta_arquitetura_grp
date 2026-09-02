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
            services.AddScoped<Interfaces.IOrganizacaoServico>(sp =>
            {
                var repositorio = sp.GetRequiredService<Repositorios.Interfaces.IOrganizacaoRepositorio>();
                var pessoaRepositorio = sp.GetRequiredService<Repositorios.Interfaces.IPessoaRepositorio>();
                return new OrganizacaoServico(repositorio, pessoaRepositorio);
            });
            services.AddScoped<Interfaces.IUsuarioServico, UsuarioServico>();
            services.AddScoped<Interfaces.IPermissionService, PermissionService>();
            services.AddScoped<Interfaces.IOrquestracaoFluxoProcessoServico, OrquestracaoFluxoProcessoServico>();
            services.AddScoped<Interfaces.IPaisServico, PaisServico>();
            services.AddScoped<Interfaces.IUfServico, UfServico>();
            services.AddScoped<Interfaces.IMunicipioServico, MunicipioServico>();
            services.AddScoped<Interfaces.IBairroServico, BairroServico>();
            services.AddScoped<Interfaces.ITipoServico, TipoServico>();
            services.AddScoped<Interfaces.IPerfilServico, PerfilServico>();
            services.AddScoped<Interfaces.ISituacaoServico, SituacaoServico>();
            services.AddScoped<Interfaces.ICepServico, CepServico>();
            services.AddScoped<Interfaces.ILogradouroServico, LogradouroServico>();
            services.AddScoped<Interfaces.IImovelServico, ImovelServico>();
            services.AddScoped<Interfaces.IPessoaServico, PessoaServico>();
            services.AddScoped<Interfaces.IPessoaFisicaServico>(sp =>
            {
                var pessoaRepo = sp.GetRequiredService<Repositorios.Interfaces.IPessoaRepositorio>();
                return new PessoaFisicaServico(pessoaRepo);
            });
            services.AddScoped<Interfaces.IPessoaJuridicaServico>(sp =>
            {
                var pessoaRepo = sp.GetRequiredService<Repositorios.Interfaces.IPessoaRepositorio>();
                return new PessoaJuridicaServico(pessoaRepo);
            });
            services.AddScoped<Interfaces.IEnderecoServico, EnderecoServico>();
            services.AddScoped<Interfaces.IOrganizacaoUnidadeServico>(sp =>
            {
                var repositorio = sp.GetRequiredService<Repositorios.Interfaces.IOrganizacaoUnidadeRepositorio>();
                var pessoaRepositorio = sp.GetRequiredService<Repositorios.Interfaces.IPessoaRepositorio>();
                return new OrganizacaoUnidadeServico(repositorio, pessoaRepositorio);
            });
            services.AddScoped<Interfaces.IOrganizacaoUnidadeSetorServico, OrganizacaoUnidadeSetorServico>();
            services.AddScoped<RequisicaoUsuario>();
            services.AddScoped<EscopoEmExecucao>();
            return services;
        }
    }
}
