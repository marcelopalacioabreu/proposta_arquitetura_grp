using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Servicos.Interfaces
{
    public interface IUsuarioServico
    {
        Task<Usuario?> AutenticarAsync(string username, string password);
        Task<Usuario> CriarUsuarioAsync(string username, string senha, string nome, string? email = null);
        Task<Usuario?> ObterPorIdAsync(long id);
    }
}
