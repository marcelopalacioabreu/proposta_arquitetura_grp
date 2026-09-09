using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Repositorios
{
    public class LogradouroRepositorio : RepositorioBase<EnderecoLogradouro>, ILogradouroRepositorio
    {
        public LogradouroRepositorio(IApplicationDbContext db) : base(db)
        {
        }

        /// <summary>
        /// Listagem com Bairro carregado
        /// </summary>
        public async Task<(List<EnderecoLogradouro> Items, int Total)> ListarComBairroAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var q = _dbSet.Include(l => l.Bairro).AsQueryable();
            q = AplicarFiltroMultilocatario(q);

            var propAtivo = typeof(EnderecoLogradouro).GetProperty("Ativo");
            if (inativo.HasValue && inativo.Value == 1)
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == false);
            else
                if (propAtivo != null) q = q.Where(e => EF.Property<bool>(e, "Ativo") == true);

            if (!string.IsNullOrWhiteSpace(nomeFilter))
                q = q.Where(e => EF.Functions.Like(EF.Property<string>(e, "Nome"), $"%{nomeFilter}%"));

            if (!string.IsNullOrWhiteSpace(sortField))
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(e => e.Nome) : q.OrderBy(e => e.Nome);
                else if (sortField == "id") q = sortDir == "desc" ? q.OrderByDescending(e => e.Id) : q.OrderBy(e => e.Id);
            else
                q = q.OrderBy(e => e.Id);

            var total = await q.CountAsync();
            var items = await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, total);
        }
    }
}
