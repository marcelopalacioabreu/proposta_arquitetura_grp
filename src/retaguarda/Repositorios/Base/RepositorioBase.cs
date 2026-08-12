using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Repositorios.Base
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly IApplicationDbContext _db;
        protected readonly DbSet<T> _dbSet;
        protected readonly IHttpContextAccessor _httpContextAccessor;

        public RepositorioBase(IApplicationDbContext db, IHttpContextAccessor httpContextAccessor = null)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            
            // Find the DbSet<T> property by checking the generic type parameter
            var dbSetProperty = _db.GetType()
                .GetProperties()
                .FirstOrDefault(p => 
                    p.PropertyType.IsGenericType && 
                    p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>) &&
                    p.PropertyType.GetGenericArguments()[0] == typeof(T));
            
            _dbSet = (DbSet<T>)(dbSetProperty?.GetValue(_db) ?? throw new System.InvalidOperationException($"DbSet for {typeof(T).Name} not found on IApplicationDbContext"));
        }
        
        /// <summary>
        /// Obtém o OrganizacaoId do escopo atual via HttpContext
        /// </summary>
        protected long? ObterOrganizacaoIdDoEscopo()
        {
            if (_httpContextAccessor?.HttpContext == null) return null;
            
            var ctx = _httpContextAccessor.HttpContext;
            if (ctx.Items.TryGetValue("escopo.organizacaoId", out var orgIdObj) && orgIdObj is long orgId)
            {
                return orgId;
            }
            return null;
        }
        
        /// <summary>
        /// Aplica filtro de OrganizacaoId automaticamente para MultilocatarioEntidade
        /// </summary>
        protected IQueryable<T> AplicarFiltroMultilocatario(IQueryable<T> query)
        {
            // Apenas filtra se T é MultilocatarioEntidade
            if (!typeof(MultilocatarioEntidade).IsAssignableFrom(typeof(T)))
                return query;
            
            var orgId = ObterOrganizacaoIdDoEscopo();
            if (!orgId.HasValue)
                return query; // Se não há escopo definido, retorna sem filtrar
            
            return query.Where(e => EF.Property<long?>(e, "OrganizacaoId") == orgId.Value);
        }

        public virtual async Task<T?> ObterPorIdAsync(long id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return null;
            
            // Validar acesso multilocatário
            if (typeof(MultilocatarioEntidade).IsAssignableFrom(typeof(T)))
            {
                var orgIdEscopo = ObterOrganizacaoIdDoEscopo();
                if (orgIdEscopo.HasValue)
                {
                    var orgIdEntity = (long?)typeof(T).GetProperty("OrganizacaoId")?.GetValue(entity);
                    if (orgIdEntity != orgIdEscopo.Value)
                        return null; // Acesso negado
                }
            }
            
            return entity;
        }

        public virtual async Task<(List<T> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet.AsQueryable();
            
            // Aplicar filtro multilocatário PRIMEIRO
            q = AplicarFiltroMultilocatario(q);

            // Try filter by `Ativo` or `Inativo` properties if exist
            var propAtivo = typeof(T).GetProperty("Ativo");
            var propInativo = typeof(T).GetProperty("Inativo");
            if (inativo.HasValue && inativo.Value == 1)
            {
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == false);
            }
            else
            {
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == true);
            }

            // Generic nome filter: try to find a `Nome` property
            if (!string.IsNullOrWhiteSpace(nomeFilter))
            {
                var propNome = typeof(T).GetProperty("Nome");
                if (propNome != null)
                {
                    q = q.Where(e => EF.Functions.Like(EF.Property<string>(e, "Nome"), $"%{nomeFilter}%"));
                }
            }

            // Simple sorting support
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                // Only support sort by 'Nome' or 'Id' by default
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(e => EF.Property<object>(e, "Nome")) : q.OrderBy(e => EF.Property<object>(e, "Nome"));
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(e => EF.Property<object>(e, "Id")) : q.OrderBy(e => EF.Property<object>(e, "Id"));
            }
            else
            {
                // fallback ordering by Id if exists
                var propId = typeof(T).GetProperty("Id");
                if (propId != null) q = q.OrderBy(e => EF.Property<object>(e, "Id"));
            }

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        public virtual async Task<(List<T> Items, int Total)> ListarAsync(Retaguarda.DTO.Parametros.PesquisaParametrosDto parametros)
        {
            var nome = parametros?.Nome;
            var page = parametros?.Pagina ?? 1;
            var pageSize = parametros?.TamanhoPagina ?? 10;
            var sortField = parametros?.SortField;
            var sortDir = parametros?.SortDir;
            var filtros = parametros?.Filtros;
            var inativo = parametros?.Inativo;
            return await ListarAsync(nome, page, pageSize, sortField, sortDir, filtros, inativo);
        }

        public virtual async Task<T> AdicionarAsync(T entity)
        {
            _dbSet.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(long id)
        {
            var e = await _dbSet.FindAsync(id);
            if (e != null)
            {
                var propAtivo = typeof(T).GetProperty("Ativo");
                if (propAtivo != null) propAtivo.SetValue(e, false);
                await _db.SaveChangesAsync();
            }
        }

        public virtual async Task RestaurarAsync(long id)
        {
            var e = await _dbSet.FindAsync(id);
            if (e != null)
            {
                var propAtivo = typeof(T).GetProperty("Ativo");
                if (propAtivo != null) propAtivo.SetValue(e, true);
                await _db.SaveChangesAsync();
            }
        }
    }
}
