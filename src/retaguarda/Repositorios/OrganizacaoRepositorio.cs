using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;

namespace Retaguarda.Repositorios
{
    public class OrganizacaoRepositorio : RepositorioBase<Organizacao>, IOrganizacaoRepositorio
    {
        public OrganizacaoRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        // Keep specific filtering behavior for Organizacao
        public override async Task<(List<Organizacao> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null)
        {
            var q = _db.Organizacoes
                .Include(o => o.Pessoa)
                .AsQueryable();

            // ativo / inativo handling (default: show ativos)
            if (inativo.HasValue && inativo.Value == 1)
                q = q.Where(o => !o.Ativo);
            else
                q = q.Where(o => o.Ativo);

            // basic name filter
            if (!string.IsNullOrWhiteSpace(nomeFilter)) q = q.Where(o => o.Nome.Contains(nomeFilter));

            // advanced single-field filter via filtros dictionary
            if (filtros != null && filtros.Count > 0)
            {
                if (filtros.TryGetValue("campo", out var campo) && filtros.TryGetValue("operador", out var operador))
                {
                    filtros.TryGetValue("valor", out var valor);
                    filtros.TryGetValue("valor_de", out var valorDe);
                    filtros.TryGetValue("valor_ate", out var valorAte);

                    var v = valor;
                    if (!string.IsNullOrWhiteSpace(campo))
                    {
                        if (campo == "nome")
                        {
                            if (operador == "iniciando_com") q = q.Where(o => o.Nome.StartsWith(v));
                            else if (operador == "contendo") q = q.Where(o => o.Nome.Contains(v));
                            else if (operador == "terminando_com") q = q.Where(o => o.Nome.EndsWith(v));
                            else if (operador == "igual") q = q.Where(o => o.Nome == v);
                        }
                        else if (campo == "created_at")
                        {
                            if (DateTime.TryParse(v, out var dt))
                            {
                                if (operador == "igual") q = q.Where(o => o.DataInsercao.Date == dt.Date);
                                else if (operador == "antes") q = q.Where(o => o.DataInsercao.Date < dt.Date);
                                else if (operador == "depois") q = q.Where(o => o.DataInsercao.Date > dt.Date);
                            }
                        }
                        else if (operador == "entre" && !string.IsNullOrWhiteSpace(valorDe) && !string.IsNullOrWhiteSpace(valorAte))
                        {
                            if (campo == "created_at" && DateTime.TryParse(valorDe, out var dtDe) && DateTime.TryParse(valorAte, out var dtAte))
                            {
                                q = q.Where(o => o.DataInsercao.Date >= dtDe.Date && o.DataInsercao.Date <= dtAte.Date);
                            }
                        }
                    }
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

        public override async Task<Organizacao?> ObterPorIdAsync(long id)
        {
            var entity = await _db.Organizacoes
                .Include(o => o.Pessoa)
                .FirstOrDefaultAsync(o => o.Id == id);
            return entity;
        }
    }
}
