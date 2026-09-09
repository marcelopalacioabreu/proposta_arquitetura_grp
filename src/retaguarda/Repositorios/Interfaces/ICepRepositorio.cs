using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface ICepRepositorio : IRepositorioBase<EnderecoCEP>
    {
        /// <summary>
        /// Listagem com Logradouro carregado
        /// </summary>
        Task<(List<EnderecoCEP> Items, int Total)> ListarComLogradouroAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null);

        /// <summary>
        /// Obtém um CEP pelo código com todos os relacionamentos carregados
        /// </summary>
        Task<EnderecoCEP?> ObterPorCodigoComLogradouroAsync(string codigo);
    }
}
