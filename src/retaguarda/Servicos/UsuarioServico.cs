using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Servicos.Util;

namespace Retaguarda.Servicos
{
    public class UsuarioServico : IUsuarioServico
    {
        private readonly IUsuarioRepositorio _repositorio;

        public UsuarioServico(IUsuarioRepositorio repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task<Usuario?> AutenticarAsync(string username, string password)
        {
            var u = await _repositorio.ObterPorUsernameAsync(username);
            if (u == null) return null;
            if (PasswordHasher.Verify(password, u.SenhaHash)) return u;
            return null;
        }

        public async Task<Usuario> CriarUsuarioAsync(string username, string senha, string nome, string? email = null)
        {
            var u = new Usuario { Username = username, Nome = nome, Email = email };
            u.SenhaHash = PasswordHasher.Hash(senha);
            return await _repositorio.AdicionarAsync(u);
        }

        public async Task<Usuario?> ObterPorIdAsync(long id)
        {
            return await _repositorio.ObterPorIdAsync(id);
        }
    }
}
