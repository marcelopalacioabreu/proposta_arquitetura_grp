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
    public class TipoRepositorio : RepositorioBase<Tipo>, ITipoRepositorio
    {
        public TipoRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        public async Task<(List<Tipo> Items, int Total)> ListarPorContextoAsync(
            string? filtroNome, 
            string? contexto,
            int page = 1, 
            int pageSize = 10, 
            string? sortField = null, 
            string? sortDir = null, 
            Dictionary<string, object>? filtros = null, 
            bool inativo = false)
        {
            var q = _dbSet.AsQueryable();
            
            // Aplicar filtro multi-locatário
            q = AplicarFiltroMultilocatario(q);
            
            // Filtrar por ativo/inativo
            if (!inativo)
                q = q.Where(x => x.Ativo);
            
            // Filtrar por contexto
            if (!string.IsNullOrWhiteSpace(contexto))
                q = q.Where(x => x.Contexto == contexto);
            
            // Filtrar por nome
            if (!string.IsNullOrWhiteSpace(filtroNome))
                q = q.Where(x => x.Nome.Contains(filtroNome) || x.Codigo.Contains(filtroNome));
            
            // Contar total
            var total = await q.CountAsync();
            
            // Ordenação
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                var isDescending = sortDir?.ToLower() == "desc";
                q = sortField.ToLower() switch
                {
                    "nome" => isDescending ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome),
                    "codigo" => isDescending ? q.OrderByDescending(x => x.Codigo) : q.OrderBy(x => x.Codigo),
                    "contexto" => isDescending ? q.OrderByDescending(x => x.Contexto) : q.OrderBy(x => x.Contexto),
                    _ => q.OrderBy(x => x.Id)
                };
            }
            else
            {
                q = q.OrderBy(x => x.Nome);
            }
            
            // Paginação
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            
            return (items, total);
        }
    }
}
