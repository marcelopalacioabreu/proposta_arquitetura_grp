using System.Threading.Tasks;
using System.Collections.Generic;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Persistencia;
using Retaguarda.Repositorios.Base;

namespace Retaguarda.Repositorios
{
    public class TipoImovelRepositorio : RepositorioBase<TipoImovel>, ITipoImovelRepositorio
    {
        public TipoImovelRepositorio(IApplicationDbContext db) : base(db)
        {
        }
    }
}
