using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir)
        {
            var q = _db.Organizacoes.AsQueryable();
            if (!string.IsNullOrWhiteSpace(nomeFilter)) q = q.Where(o => o.Nome.Contains(nomeFilter));

            // sorting
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(o => o.Nome) : q.OrderBy(o => o.Nome);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(o => o.Id) : q.OrderBy(o => o.Id);
            }
            else
            {
                q = q.OrderBy(o => o.Nome);
            }

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        public async Task<Organizacao> AdicionarAsync(Organizacao o)
        {
            _db.Organizacoes.Add(o);
            await _db.SaveChangesAsync();
            return o;
        }

        public async Task DeleteAsync(long id)
        {
            var e = await _db.Organizacoes.FindAsync(id);
            if (e != null) { _db.Organizacoes.Remove(e); await _db.SaveChangesAsync(); }
        }

        public async Task UpdateAsync(Organizacao o)
        {
            _db.Organizacoes.Update(o);
            await _db.SaveChangesAsync();
        }
    }
}
