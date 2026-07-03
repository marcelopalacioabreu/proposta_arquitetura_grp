using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Servicos.Interfaces
{
    public interface IOrganizacaoServico
    {
        Task<Organizacao?> ObterPorIdAsync(long id);
        Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null);
        Task<Organizacao> CriarAsync(string nome);
        Task DeleteAsync(long id);
        Task UpdateAsync(Organizacao o);
    }
}
