using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IOrganizacaoRepositorio
    {
        Task<Organizacao?> ObterPorIdAsync(long id);
        Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir);
        Task<Organizacao> AdicionarAsync(Organizacao o);
        Task DeleteAsync(long id);
        Task UpdateAsync(Organizacao o);
    }
}
