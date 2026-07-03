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

        public async Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null)
        {
            return await _repositorio.ListarAsync(nomeFilter, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte, inativo);
        }

        public async Task<Organizacao> CriarAsync(string nome)
        {
            var o = new Organizacao { Nome = nome };
            return await _repositorio.AdicionarAsync(o);
        }

        public async Task DeleteAsync(long id)
        {
            await _repositorio.DeleteAsync(id);
        }

        public async Task UpdateAsync(Organizacao o)
        {
            await _repositorio.UpdateAsync(o);
        }
    }
}
