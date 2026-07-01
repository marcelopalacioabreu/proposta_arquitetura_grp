using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IOrganizacaoRepositorio
    {
        Task<Organizacao?> ObterPorIdAsync(long id);
    }
}
