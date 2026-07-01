using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("meta")]
    public class MetaController : ControllerBase
    {
        private readonly string _metaPath;

        public MetaController(IWebHostEnvironment env)
        {
            // DOCUMENTACAO/METADADOS at repo root
            _metaPath = Path.GetFullPath(Path.Combine(env.ContentRootPath, "..", "..", "..", "..", "DOCUMENTACAO", "METADADOS"));
        }

        [HttpGet("routes")]
        public IActionResult Routes()
        {
            var p = Path.Combine(_metaPath, "routes.json");
            if (!System.IO.File.Exists(p)) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("components")]
        public IActionResult Components()
        {
            var p = Path.Combine(_metaPath, "components.json");
            if (!System.IO.File.Exists(p)) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("screens")]
        public IActionResult Screens()
        {
            var p = Path.Combine(_metaPath, "screens.json");
            if (!System.IO.File.Exists(p)) return NotFound();
            var json = System.IO.File.ReadAllText(p);
            return Content(json, "application/json");
        }

        [HttpGet("all")]
        public IActionResult All()
        {
            var routes = System.IO.File.Exists(Path.Combine(_metaPath, "routes.json")) ? System.IO.File.ReadAllText(Path.Combine(_metaPath, "routes.json")) : "{}";
            var comps = System.IO.File.Exists(Path.Combine(_metaPath, "components.json")) ? System.IO.File.ReadAllText(Path.Combine(_metaPath, "components.json")) : "{}";
            var screens = System.IO.File.Exists(Path.Combine(_metaPath, "screens.json")) ? System.IO.File.ReadAllText(Path.Combine(_metaPath, "screens.json")) : "{}";

            // Return combined object parsed from JSON files
            return Ok(new { routes = System.Text.Json.JsonDocument.Parse(routes).RootElement, components = System.Text.Json.JsonDocument.Parse(comps).RootElement, screens = System.Text.Json.JsonDocument.Parse(screens).RootElement });
        }
    }
}
