using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.Servicos
{
    public class OrganizacaoServico : IOrganizacaoServico
    {
        private readonly IOrganizacaoRepositorio _repositorio;

        public OrganizacaoServico(IOrganizacaoRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Organizacao?> ObterPorIdAsync(long id)
        {
            return await _repositorio.ObterPorIdAsync(id);
        }
    }
}
