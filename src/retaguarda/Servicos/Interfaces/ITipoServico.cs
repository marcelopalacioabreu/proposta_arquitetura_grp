using Retaguarda.DTO.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retaguarda.Servicos.Interfaces
{
    public interface ITipoServico : IServicoBase<TipoDto>
    {
        Task<(List<TipoDto> Items, int Total)> ListarPorContextoAsync(
            string? filtroNome, 
            string? contexto,
            int page = 1, 
            int pageSize = 10, 
            string? sortField = null, 
            string? sortDir = null, 
            Dictionary<string, object>? filtros = null, 
            bool inativo = false);
    }
}
