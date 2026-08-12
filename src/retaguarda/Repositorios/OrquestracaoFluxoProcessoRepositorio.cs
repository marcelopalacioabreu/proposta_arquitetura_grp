using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Retaguarda.Repositorios
{
    public class OrquestracaoFluxoProcessoRepositorio : Retaguarda.Repositorios.Base.RepositorioBase<OrquestracaoFluxoProcesso>, IOrquestracaoFluxoProcessoRepositorio
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrquestracaoFluxoProcessoRepositorio(Retaguarda.Persistencia.IApplicationDbContext db, IHttpContextAccessor httpContextAccessor) 
            : base(db, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Extrai OrganizacaoId do contexto HTTP (injetado via middleware)
        /// </summary>
        private long? ObterOrganizacaoIdDoContexto()
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context?.Items.TryGetValue("OrganizacaoId", out var orgId) == true && orgId is long id)
            {
                return id;
            }
            return null;
        }

        /// <summary>
        /// Sobrescreve ListarAsync para aplicar isolamento automático por OrganizacaoId
        /// </summary>
        public override async Task<(List<OrquestracaoFluxoProcesso> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, 
            System.Collections.Generic.IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var q = _db.OrquestracaoFluxoProcessos.AsQueryable();

            // Isolamento multilocatário: filtrar por OrganizacaoId do contexto
            var orgId = ObterOrganizacaoIdDoContexto();
            if (orgId.HasValue)
            {
                q = q.Where(x => x.OrganizacaoId == orgId.Value);
            }

            // ativo / inativo handling (default: show ativos)
            if (inativo.HasValue && inativo.Value == 1)
                q = q.Where(x => !x.Ativo);
            else
                q = q.Where(x => x.Ativo);

            // basic name filter
            if (!string.IsNullOrWhiteSpace(nomeFilter))
                q = q.Where(x => x.Nome.Contains(nomeFilter));

            // sorting
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(x => x.Id) : q.OrderBy(x => x.Id);
            }
            else
            {
                q = q.OrderBy(x => x.Nome);
            }

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        /// <summary>
        /// Sobrescreve ObterPorIdAsync para validar que a entidade pertence à organização atual
        /// </summary>
        public override async Task<OrquestracaoFluxoProcesso?> ObterPorIdAsync(long id)
        {
            var entity = await base.ObterPorIdAsync(id);
            if (entity == null) return null;

            // Validar isolamento multilocatário
            var orgId = ObterOrganizacaoIdDoContexto();
            if (orgId.HasValue && entity.OrganizacaoId != orgId.Value)
            {
                return null; // Acesso negado: não pertence à organização atual
            }

            return entity;
        }

        /// <summary>
        /// Sobrescreve DeleteAsync para validar isolamento
        /// </summary>
        public override async Task DeleteAsync(long id)
        {
            var entity = await base.ObterPorIdAsync(id);
            if (entity == null) return;

            // Validar isolamento multilocatário
            var orgId = ObterOrganizacaoIdDoContexto();
            if (orgId.HasValue && entity.OrganizacaoId != orgId.Value)
            {
                throw new UnauthorizedAccessException($"Não tem permissão para deletar este registro. OrganizacaoId diferente.");
            }

            await base.DeleteAsync(id);
        }

        /// <summary>
        /// Sobrescreve RestaurarAsync para validar isolamento
        /// </summary>
        public override async Task RestaurarAsync(long id)
        {
            var entity = await base.ObterPorIdAsync(id);
            if (entity == null) return;

            // Validar isolamento multilocatário
            var orgId = ObterOrganizacaoIdDoContexto();
            if (orgId.HasValue && entity.OrganizacaoId != orgId.Value)
            {
                throw new UnauthorizedAccessException($"Não tem permissão para restaurar este registro. OrganizacaoId diferente.");
            }

            await base.RestaurarAsync(id);
        }
    }
}
