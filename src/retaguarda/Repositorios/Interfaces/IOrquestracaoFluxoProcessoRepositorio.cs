using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IOrquestracaoFluxoProcessoRepositorio
    {
        Task<OrquestracaoFluxoProcesso?> ObterPorIdAsync(long id);
        Task<(List<OrquestracaoFluxoProcesso> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null);
        Task<OrquestracaoFluxoProcesso> AdicionarAsync(OrquestracaoFluxoProcesso o);
        Task DeleteAsync(long id);
        Task RestaurarAsync(long id);
        Task UpdateAsync(OrquestracaoFluxoProcesso o);
    }
}
