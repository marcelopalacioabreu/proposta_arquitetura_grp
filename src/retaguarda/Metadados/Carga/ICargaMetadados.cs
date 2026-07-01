using System.Collections.Generic;
using Retaguarda.Metadados.Contracts;

namespace Retaguarda.Metadados.Loaders
{
    public interface ICargaMetadados
    {
        IEnumerable<RouteDefinition> CarregarRotas();
        IEnumerable<ComponentDefinition> CarregarComponentes();
        IEnumerable<ScreenDefinition> CarregarTelas();
    }
}
