using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Servicos.Interfaces
{
    public interface IServicoBase<TDto>
    {
        Task<TDto?> ObterPorIdAsync(long id);
        Task<(List<TDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null);
        Task<(List<TDto> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros);
        Task<TDto> CriarAsync(TDto dto);
        Task UpdateAsync(long id, TDto dto);
        Task DeleteAsync(long id);
        Task RestaurarAsync(long id);
    }
}
