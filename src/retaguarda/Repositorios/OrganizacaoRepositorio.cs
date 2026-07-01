using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia.MYSQL;

namespace Retaguarda.Repositorios
{
    public class OrganizacaoRepositorio : IOrganizacaoRepositorio
    {
        private readonly ApplicationDbContext _db;

        public OrganizacaoRepositorio(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Organizacao?> ObterPorIdAsync(long id)
        {
            return await _db.Organizacoes.FindAsync(id);
        }
    }
}
