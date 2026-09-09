using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using System.Threading.Tasks;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface ICepRepositorio : IRepositorioBase<EnderecoCEP>
    {
        /// <summary>
        /// Obtém um CEP pelo código com todos os relacionamentos carregados
        /// </summary>
        Task<EnderecoCEP?> ObterPorCodigoComLogradouroAsync(string codigo);
    }
}
