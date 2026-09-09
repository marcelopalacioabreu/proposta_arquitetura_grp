using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface ILogradouroRepositorio : IRepositorioBase<EnderecoLogradouro>
    {
        /// <summary>
        /// Listagem com Bairro carregado
        /// </summary>
        Task<(List<EnderecoLogradouro> Items, int Total)> ListarComBairroAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null);
    }
}
