using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface IUsuarioRepositorio
    {
        Task<Usuario?> ObterPorIdAsync(long id);
        Task<Usuario?> ObterPorUsernameAsync(string username);
        Task<Usuario> AdicionarAsync(Usuario usuario);
        Task<Usuario> AtualizarAsync(Usuario usuario);
    }
}
