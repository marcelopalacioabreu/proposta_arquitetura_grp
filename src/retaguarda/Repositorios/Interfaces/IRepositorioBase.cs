using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<T?> ObterPorIdAsync(long id);
        Task<(List<T> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null);
        Task<(List<T> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros);
        Task<T> AdicionarAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long id);
        Task RestaurarAsync(long id);
    }
}
