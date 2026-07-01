using System.Collections.Generic;

namespace Retaguarda.Metadados.Contracts
{
    public class RouteDefinition
    {
        public string Id { get; set; } = null!;
        public string Path { get; set; } = null!;
        public string Verb { get; set; } = "GET";
        public string? Controller { get; set; }
        public string? Action { get; set; }
        public List<string>? Middlewares { get; set; }
        public string? Permission { get; set; }
        public string? Module { get; set; }
    }
}
