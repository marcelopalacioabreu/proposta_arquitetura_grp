using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;
using Microsoft.EntityFrameworkCore;

namespace Retaguarda.Repositorios
{
    public class OrganizacaoUnidadeSetorRepositorio : RepositorioBase<OrganizacaoUnidadeSetor>, IOrganizacaoUnidadeSetorRepositorio
    {
        public OrganizacaoUnidadeSetorRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        public override async Task<(List<OrganizacaoUnidadeSetor> Items, int Total)> ListarAsync(
            string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir,
            IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet
                .Include(x => x.OrganizacaoUnidade)
                .AsQueryable();
            q = AplicarFiltroMultilocatario(q);

            if (filtros != null && filtros.TryGetValue("organizacaoUnidadeId", out var unidadeIdStr)
                && long.TryParse(unidadeIdStr, out var unidadeId))
                q = q.Where(e => EF.Property<long?>(e, "OrganizacaoUnidadeId") == unidadeId);

            if (inativo.HasValue && inativo.Value == 1)
                q = q.Where(e => !e.Ativo);
            else
                q = q.Where(e => e.Ativo);

            if (!string.IsNullOrWhiteSpace(nomeFilter))
                q = q.Where(e => EF.Functions.Like(e.Nome, $"%{nomeFilter}%"));

            q = q.OrderBy(e => e.Nome);
            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }
    }
}
