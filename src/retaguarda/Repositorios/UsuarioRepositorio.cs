using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Retaguarda.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public UsuarioRepositorio(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Usuario?> ObterPorIdAsync(long id)
        {
            return await _db.Usuarios.FindAsync(id);
        }

        //public async Task<Usuario?> ObterPorUsernameAsync(string username)
        //{
        //    return await _db.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        //}

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            _db.Usuarios.Add(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> AtualizarAsync(Usuario usuario)
        {
            _db.Usuarios.Update(usuario);
            await _db.SaveChangesAsync();
            return usuario;
        }
    }
}
