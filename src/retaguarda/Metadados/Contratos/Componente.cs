using System.Collections.Generic;

namespace Retaguarda.Metadados.Contracts
{
    public class Componente
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!; // e.g., TabelaPaginavel, Filtro
        public Dictionary<string, object>? Parameters { get; set; }
        public string? Module { get; set; }
    }
}
