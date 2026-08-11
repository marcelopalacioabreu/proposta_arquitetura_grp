using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Interfaces;

namespace Retaguarda.Repositorios.Base
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {
        protected readonly IApplicationDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public RepositorioBase(IApplicationDbContext db)
        {
            _db = db;
            _dbSet = (DbSet<T>)_db.GetType().GetProperty(typeof(T).Name + "s")?.GetValue(_db) ?? throw new System.InvalidOperationException($"DbSet for {typeof(T).Name} not found on IApplicationDbContext");
        }

        public virtual async Task<T?> ObterPorIdAsync(long id) => await _dbSet.FindAsync(id);

        public virtual async Task<(List<T> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet.AsQueryable();

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
