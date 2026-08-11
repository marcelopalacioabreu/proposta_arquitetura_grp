using System.Threading.Tasks;
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
        public OrquestracaoFluxoProcessoRepositorio(Retaguarda.Persistencia.IApplicationDbContext db) : base(db)
        {
        }

        // No custom behavior for now; reuse base generic implementations.
    }
}
