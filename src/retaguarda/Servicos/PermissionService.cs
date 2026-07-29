using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.Servicos
{
    public class PermissionService : IPermissionService
    {
        private readonly ApplicationDbContext _db;

        public PermissionService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IList<string>> GetPermissionsForUserAsync(long userId)
        {
            var perfilIds = await _db.PerfilUsuarios.Where(pu => pu.UsuarioId == userId).Select(pu => pu.PerfilId).ToListAsync();
            if (!perfilIds.Any()) return new List<string>();
            var perms = await _db.PerfilPermissoes.Where(pp => perfilIds.Contains(pp.PerfilId)).Select(pp => pp.Nome).ToListAsync();
            return perms.Distinct().ToList();
        }

        public async Task<bool> IsUserAdministratorAsync(long userId)
        {
            var perfilIds = await _db.PerfilUsuarios.Where(pu => pu.UsuarioId == userId).Select(pu => pu.PerfilId).ToListAsync();
            if (!perfilIds.Any()) return false;
            return await _db.Perfis.AnyAsync(p => perfilIds.Contains(p.Id) && p.AdministradorDoSistema);
        }
    }
}
