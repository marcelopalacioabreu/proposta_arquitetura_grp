using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Servicos.Interfaces
{
    public interface IOrganizacaoServico
    {
        Task<Organizacao?> ObterPorIdAsync(long id);
    }
}
