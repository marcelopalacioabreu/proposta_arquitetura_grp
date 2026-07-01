using System.Collections.Generic;

namespace Retaguarda.Metadados.Contracts
{
    public class ScreenDefinition
    {
        public string RouteId { get; set; } = null!; // associates screen to a route
        public string Title { get; set; } = null!;
        public List<string>? Components { get; set; } // names of components to render
        public string? Module { get; set; }
    }
}
