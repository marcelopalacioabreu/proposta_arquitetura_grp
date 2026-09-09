using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IMunicipioRepositorio : IRepositorioBase<EnderecoMunicipio>
    {
        /// <summary>
        /// Listagem com UF carregado
        /// </summary>
        Task<(List<EnderecoMunicipio> Items, int Total)> ListarComUfAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null);
    }
}
