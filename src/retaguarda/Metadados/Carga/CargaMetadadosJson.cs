using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Retaguarda.Metadados.Contracts;

namespace Retaguarda.Metadados.Loaders
{
    public class CargaMetadadosJson : ICargaMetadados
    {
        private readonly string _basePath;

        public CargaMetadadosJson(string basePath)
        {
            _basePath = basePath;
        }

        public IEnumerable<RouteDefinition> CarregarRotas()
        {
            var path = Path.Combine(_basePath, "routes.json");
            if (!File.Exists(path)) return new List<RouteDefinition>();
            using var s = File.OpenRead(path);
            var doc = JsonSerializer.Deserialize<RoutesFile>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
            return doc?.Routes ?? new List<RouteDefinition>();
        }

        public IEnumerable<ComponentDefinition> CarregarComponentes()
        {
            var path = Path.Combine(_basePath, "components.json");
            if (!File.Exists(path)) return new List<ComponentDefinition>();
            using var s = File.OpenRead(path);
            var doc = JsonSerializer.Deserialize<ComponentsFile>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
            return doc?.Components ?? new List<ComponentDefinition>();
        }

        public IEnumerable<ScreenDefinition> CarregarTelas()
        {
            var path = Path.Combine(_basePath, "screens.json");
            if (!File.Exists(path)) return new List<ScreenDefinition>();
            using var s = File.OpenRead(path);
            var doc = JsonSerializer.Deserialize<ScreensFile>(s, new JsonSerializerOptions{PropertyNameCaseInsensitive=true});
            return doc?.Screens ?? new List<ScreenDefinition>();
        }

        private class RoutesFile { public List<RouteDefinition>? Routes { get; set; } }
        private class ComponentsFile { public List<ComponentDefinition>? Components { get; set; } }
        private class ScreensFile { public List<ScreenDefinition>? Screens { get; set; } }
    }
}
