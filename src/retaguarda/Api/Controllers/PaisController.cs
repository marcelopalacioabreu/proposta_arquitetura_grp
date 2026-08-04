using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/paises")]
    public class PaisController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public PaisController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Paises.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q) || x.Codigo.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, x.Codigo })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var p = _db.Paises.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            return OkData(p);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Pais dto)
        {
            _db.Paises.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Pais dto)
        {
            var p = _db.Paises.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            p.Nome = dto.Nome ?? p.Nome;
            p.Codigo = dto.Codigo ?? p.Codigo;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var p = _db.Paises.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            p.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
