using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/logradouros")]
    public class LogradouroController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public LogradouroController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Logradouros.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, x.Tipo, BairroNome = x.Bairro != null ? x.Bairro.Nome : string.Empty, x.BairroId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var l = _db.Logradouros.Find(id);
            if (l == null) return NotFoundError("Registro não encontrado");
            return OkData(l);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Logradouro dto)
        {
            _db.Logradouros.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Logradouro dto)
        {
            var l = _db.Logradouros.Find(id);
            if (l == null) return NotFoundError("Registro não encontrado");
            l.Nome = dto.Nome ?? l.Nome;
            l.Tipo = dto.Tipo ?? l.Tipo;
            l.BairroId = dto.BairroId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var l = _db.Logradouros.Find(id);
            if (l == null) return NotFoundError("Registro não encontrado");
            l.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
