using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IBairroRepositorio : IRepositorioBase<EnderecoBairro>
    {
        /// <summary>
        /// Listagem com Municipio carregado
        /// </summary>
        Task<(List<EnderecoBairro> Items, int Total)> ListarComMunicipioAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null);
    }
}
