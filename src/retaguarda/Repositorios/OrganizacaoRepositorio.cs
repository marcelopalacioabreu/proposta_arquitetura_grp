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

        public async Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null)
        {
            var q = _db.Organizacoes.AsQueryable();

            // ativo / inativo handling (default: show ativos)
            if (inativo.HasValue && inativo.Value == 1)
                q = q.Where(o => !o.Ativo);
            else
                q = q.Where(o => o.Ativo);

            // basic name filter
            if (!string.IsNullOrWhiteSpace(nomeFilter)) q = q.Where(o => o.Nome.Contains(nomeFilter));

            // advanced single-field filter (campo/operator/value)
            if (!string.IsNullOrWhiteSpace(campo) && !string.IsNullOrWhiteSpace(operador) && !string.IsNullOrWhiteSpace(valor))
            {
                var v = valor;
                if (campo == "nome")
                {
                    if (operador == "iniciando_com") q = q.Where(o => o.Nome.StartsWith(v));
                    else if (operador == "contendo") q = q.Where(o => o.Nome.Contains(v));
                    else if (operador == "terminando_com") q = q.Where(o => o.Nome.EndsWith(v));
                    else if (operador == "igual") q = q.Where(o => o.Nome == v);
                }
                // other fields can be added here when present in the entity
                else if (campo == "created_at")
                {
                    if (DateTime.TryParse(v, out var dt))
                    {
                        if (operador == "igual") q = q.Where(o => o.DataInsercao.Date == dt.Date);
                        else if (operador == "antes") q = q.Where(o => o.DataInsercao.Date < dt.Date);
                        else if (operador == "depois") q = q.Where(o => o.DataInsercao.Date > dt.Date);
                    }
                }
            }
            else if (!string.IsNullOrWhiteSpace(campo) && operador == "entre" && !string.IsNullOrWhiteSpace(valorDe) && !string.IsNullOrWhiteSpace(valorAte))
            {
                if (campo == "created_at" && DateTime.TryParse(valorDe, out var dtDe) && DateTime.TryParse(valorAte, out var dtAte))
                {
                    q = q.Where(o => o.DataInsercao.Date >= dtDe.Date && o.DataInsercao.Date <= dtAte.Date);
                }
            }

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
