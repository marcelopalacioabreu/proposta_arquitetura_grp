using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("meta")]
    public class MetaController : ControllerBase
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

        private string? FindFile(params string[] relativeParts)
        {
            var projectPath = Path.Combine(new [] { _projectMetaPath }.Concat(relativeParts).ToArray());
            if (System.IO.File.Exists(projectPath)) return projectPath;
            var docPath = Path.Combine(new [] { _docMetaPath }.Concat(relativeParts).ToArray());
            if (System.IO.File.Exists(docPath)) return docPath;
            return null;
        }

        [HttpGet("routes")]
        public IActionResult Routes()
        {
            var p = FindFile("routes.json") ?? FindFile("Rotas", "api.json");
            if (p == null) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("components")]
        public IActionResult Components()
        {
            var p = FindFile("components.json") ?? FindFile("Componentes", "components.json");
            if (p == null) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("modulos")]
        public IActionResult Modulos()
        {
            var p = FindFile("modulos.json") ?? FindFile("Modulos", "modulos.json");
            if (p == null) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("screens")]
        public IActionResult Screens()
        {
            var p = FindFile("screens.json") ?? FindFile("Telas", "telas.json");
            if (p == null) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            var routesPath = FindFile("routes.json") ?? FindFile("Rotas","api.json");
            var compsPath = FindFile("components.json") ?? FindFile("Componentes","components.json");
            var screensPath = FindFile("screens.json") ?? FindFile("Telas","telas.json");

            var routes = routesPath != null ? System.IO.File.ReadAllText(routesPath) : "{}";
            var comps = compsPath != null ? System.IO.File.ReadAllText(compsPath) : "{}";
            var screens = screensPath != null ? System.IO.File.ReadAllText(screensPath) : "{}";

            return Ok(new { routes = System.Text.Json.JsonDocument.Parse(routes).RootElement, components = System.Text.Json.JsonDocument.Parse(comps).RootElement, screens = System.Text.Json.JsonDocument.Parse(screens).RootElement });
        }
    }
}
