using System.Threading.Tasks;
using System.Collections.Generic;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;

namespace Retaguarda.Repositorios
{
    public class NivelGovernoRepositorio : RepositorioBase<NivelGoverno>, INivelGovernoRepositorio
    {
        public NivelGovernoRepositorio(IApplicationDbContext db) : base(db)
        {
        }
    }
}
