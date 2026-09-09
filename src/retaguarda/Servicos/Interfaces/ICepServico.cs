using Retaguarda.DTO.Dtos;
using Retaguarda.Servicos.Interfaces;
using System.Threading.Tasks;

namespace Retaguarda.Servicos.Interfaces
{
    public interface ICepServico : IServicoBase<CepDto>
    {
        /// <summary>
        /// Obtém um CEP pelo código com todos os relacionamentos carregados
        /// </summary>
        Task<CepDto?> ObterPorCodigoComLogradouroAsync(string codigo);
    }
}
