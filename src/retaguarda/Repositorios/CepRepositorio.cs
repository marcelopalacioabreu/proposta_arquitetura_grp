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
