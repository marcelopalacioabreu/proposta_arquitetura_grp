using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("meta")]
    public class MetaController : BaseController
    {
        private readonly string _projectMetaPath;
        private readonly string _docMetaPath;

        public MetaController(IWebHostEnvironment env)
        {
            // Project metadados: ../Metadados/Contratos relative to API project
            _projectMetaPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "Metadados", "Contratos"));
            // Fallback DOCUMENTACAO/METADADOS at repo root
            _docMetaPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "..", "..", "..", "DOCUMENTACAO", "METADADOS"));
        }

        private string? BuscarArquivo(params string[] relativeParts)
        {
            var projectPath = Path.Combine(new [] { _projectMetaPath }.Concat(relativeParts).ToArray());
            if (System.IO.File.Exists(projectPath)) return projectPath;
            var docPath = Path.Combine(new [] { _docMetaPath }.Concat(relativeParts).ToArray());
            if (System.IO.File.Exists(docPath)) return docPath;
            return null;
        }

        [HttpGet("rotas")]
        public IActionResult Rotas()
        {
            var p = BuscarArquivo("cliente.json") ?? BuscarArquivo("Rotas", "api.json");
            if (p == null) return NotFoundError("Arquivo não encontrado");
            var json = System.IO.File.ReadAllText(p);
            var parsed = System.Text.Json.JsonDocument.Parse(json).RootElement;
            return OkData(parsed);
        }

        [HttpGet("components")]
        public IActionResult Componentes()
        {
            var p = BuscarArquivo("componentes.json") ?? BuscarArquivo("Componentes", "componentes.json");
            if (p == null) return NotFoundError("Arquivo não encontrado");
            var json = System.IO.File.ReadAllText(p);
            var parsed = System.Text.Json.JsonDocument.Parse(json).RootElement;
            return OkData(parsed);
        }

        [HttpGet("modulos")]
        public IActionResult Modulos()
        {
            var p = BuscarArquivo("modulos.json") ?? BuscarArquivo("Modulos", "modulos.json");
            if (p == null) return NotFoundError("Arquivo não encontrado");
            var json = System.IO.File.ReadAllText(p);
            var parsed = System.Text.Json.JsonDocument.Parse(json).RootElement;
            return OkData(parsed);
        }

        [HttpGet("screens")]
        public IActionResult Telas()
        {
            // Aggregate all JSON files under Metadados/Contratos/Telas recursively
            var screensDirProject = Path.Combine(_projectMetaPath, "Telas");
            var screensDirDoc = Path.Combine(_docMetaPath, "Telas");

            var aggregated = new System.Collections.Generic.Dictionary<string, object>();

            void carregarDoDiretorio(string dir)
            {
                if (!Directory.Exists(dir)) return;
                var files = Directory.GetFiles(dir, "*.json", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    try
                    {
                        var txt = System.IO.File.ReadAllText(f);
                        var doc = System.Text.Json.JsonDocument.Parse(txt);
                        foreach (var prop in doc.RootElement.EnumerateObject())
                        {
                            aggregated[prop.Name] = System.Text.Json.JsonSerializer.Deserialize<object>(prop.Value.GetRawText());
                        }
                    }
                    catch { /* ignore malformed */ }
                }
            }

            // Load doc fallback first, then project to allow overrides
            carregarDoDiretorio(screensDirDoc);
            carregarDoDiretorio(screensDirProject);

            return OkData(aggregated);
        }

        [HttpGet("all")]
        public IActionResult todos()
        {
            var routesPath = BuscarArquivo("cliente.json") ?? BuscarArquivo("Rotas","api.json");
            var compsPath = BuscarArquivo("componentes.json") ?? BuscarArquivo("Componentes","componentes.json");
            var screensPath = BuscarArquivo("telas.json") ?? BuscarArquivo("Telas","telas.json");

            var routes = routesPath != null ? System.IO.File.ReadAllText(routesPath) : "{}";
            var comps = compsPath != null ? System.IO.File.ReadAllText(compsPath) : "{}";
            var screens = screensPath != null ? System.IO.File.ReadAllText(screensPath) : "{}";

            return OkData(new { routes = System.Text.Json.JsonDocument.Parse(routes).RootElement, components = System.Text.Json.JsonDocument.Parse(comps).RootElement, screens = System.Text.Json.JsonDocument.Parse(screens).RootElement });
        }
    }
}
