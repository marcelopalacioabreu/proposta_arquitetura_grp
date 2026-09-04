using System;
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
    public class OrganizacaoUnidadeRepositorio : RepositorioBase<OrganizacaoUnidade>, IOrganizacaoUnidadeRepositorio
    {
        public OrganizacaoUnidadeRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        public override async Task<OrganizacaoUnidade?> ObterPorIdAsync(long id)
        {
            var entity = await _dbSet
                .Include(x => x.Pessoa)
                .FirstOrDefaultAsync(x => x.Id == id);
            
            if (entity == null) return null;
            
            // Validar acesso multilocatário
            if (typeof(Dominio.Entidades.Base.MultilocatarioEntidade).IsAssignableFrom(typeof(OrganizacaoUnidade)))
            {
                var orgIdEscopo = ObterOrganizacaoIdDoEscopo();
                if (orgIdEscopo.HasValue)
                {
                    var orgIdEntity = entity.OrganizacaoId;
                    if (orgIdEntity != orgIdEscopo.Value)
                        return null; // Acesso negado
                }
            }
            
            return entity;
        }

        public override async Task<(List<OrganizacaoUnidade> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet
                .Include(x => x.Pessoa)
                .Include(x => x.Organizacao)
                .AsQueryable();
            
            // Aplicar filtro multilocatário
            q = AplicarFiltroMultilocatario(q);

            // Aplicar filtros do dicionário (ex: organizacaoId, situacaoId, etc.)
            if (filtros != null && filtros.Count > 0)
            {
                if (filtros.TryGetValue("organizacaoId", out var orgIdStr) && long.TryParse(orgIdStr, out var orgId))
                {
                    q = q.Where(e => e.OrganizacaoId == orgId);
                }
                
                if (filtros.TryGetValue("tipoId", out var tipoIdStr) && long.TryParse(tipoIdStr, out var tipoId))
                {
                    q = q.Where(e => e.TipoId == tipoId);
                }
                
                if (filtros.TryGetValue("situacaoId", out var sitIdStr) && long.TryParse(sitIdStr, out var sitId))
                {
                    q = q.Where(e => e.SituacaoId == sitId);
                }
            }

            // Try filter by `Ativo`
            var propAtivo = typeof(OrganizacaoUnidade).GetProperty("Ativo");
            if (inativo.HasValue && inativo.Value == 1)
            {
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == false);
            }
            else
            {
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == true);
            }

            // Generic nome filter
            if (!string.IsNullOrWhiteSpace(nomeFilter))
            {
                q = q.Where(e => EF.Functions.Like(e.Nome, $"%{nomeFilter}%"));
            }

            // Simple sorting support
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(e => e.Nome) : q.OrderBy(e => e.Nome);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(e => e.Id) : q.OrderBy(e => e.Id);
            }
            else
            {
                q = q.OrderBy(e => e.Id);
            }

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }
    }
}
