using System.Threading.Tasks;
using System.Collections.Generic;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Retaguarda.Repositorios
{
    public class CepRepositorio : RepositorioBase<EnderecoCEP>, ICepRepositorio
    {
        public CepRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Listagem com Logradouro carregado
        /// </summary>
        public async Task<(List<EnderecoCEP> Items, int Total)> ListarComLogradouroAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet.Include(c => c.Logradouro).AsQueryable();
            q = AplicarFiltroMultilocatario(q);

            var propAtivo = typeof(EnderecoCEP).GetProperty("Ativo");
            if (inativo.HasValue && inativo.Value == 1)
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == false);
            else
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == true);

            if (!string.IsNullOrWhiteSpace(nomeFilter))
                q = q.Where(e => EF.Functions.Like(EF.Property<string>(e, "Codigo"), $"%{nomeFilter}%"));

            if (!string.IsNullOrWhiteSpace(sortField))
                if (sortField == "codigo") q = sortDir == "desc" ? q.OrderByDescending(e => e.Codigo) : q.OrderBy(e => e.Codigo);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(e => e.Id) : q.OrderBy(e => e.Id);
            else
                q = q.OrderBy(e => e.Id);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }

        /// <summary>
        /// Obtém um CEP pelo código com todos os relacionamentos carregados
        /// </summary>
        public async Task<EnderecoCEP?> ObterPorCodigoComLogradouroAsync(string codigo)
        {
            return await _dbSet
                .Include(c => c.Logradouro)
                    .ThenInclude(l => l.Bairro)
                        .ThenInclude(b => b.Municipio)
                            .ThenInclude(m => m.Uf)
                                .ThenInclude(u => u.Pais)
                .FirstOrDefaultAsync(c => c.Codigo == codigo);
        }
    }
}
