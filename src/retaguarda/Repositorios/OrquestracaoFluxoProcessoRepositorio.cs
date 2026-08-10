using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Retaguarda.Repositorios
{
    public class OrquestracaoFluxoProcessoRepositorio : IOrquestracaoFluxoProcessoRepositorio
    {
        private readonly IApplicationDbContext _db;

        public OrquestracaoFluxoProcessoRepositorio(IApplicationDbContext db) => _db = db;

        public async Task<OrquestracaoFluxoProcesso?> ObterPorIdAsync(long id) => await _db.OrquestracaoFluxoProcessos.FindAsync(id);

        public async Task<(List<OrquestracaoFluxoProcesso> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null)
        {
            var q = _db.OrquestracaoFluxoProcessos.AsQueryable();

            if (inativo.HasValue && inativo.Value == 1) q = q.Where(x => !x.Ativo);
            else q = q.Where(x => x.Ativo);

            if (!string.IsNullOrWhiteSpace(nomeFilter)) q = q.Where(x => x.Nome.Contains(nomeFilter));

            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id);
            }
            else q = q.OrderBy(x => x.Nome);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        public async Task<OrquestracaoFluxoProcesso> AdicionarAsync(OrquestracaoFluxoProcesso o)
        {
            _db.OrquestracaoFluxoProcessos.Add(o);
            await _db.SaveChangesAsync();
            return o;
        }

        public async Task DeleteAsync(long id)
        {
            var e = await _db.OrquestracaoFluxoProcessos.FindAsync(id);
            if (e != null) { e.Ativo = false; await _db.SaveChangesAsync(); }
        }

        public async Task RestaurarAsync(long id)
        {
            var e = await _db.OrquestracaoFluxoProcessos.FindAsync(id);
            if (e != null) { e.Ativo = true; await _db.SaveChangesAsync(); }
        }

        public async Task UpdateAsync(OrquestracaoFluxoProcesso o)
        {
            _db.OrquestracaoFluxoProcessos.Update(o);
            await _db.SaveChangesAsync();
        }
    }
}
