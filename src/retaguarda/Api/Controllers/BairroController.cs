using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/bairros")]
    public class BairroController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public BairroController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Bairros.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, MunicipioNome = x.Municipio != null ? x.Municipio.Nome : string.Empty, x.MunicipioId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var b = _db.Bairros.Find(id);
            if (b == null) return NotFoundError("Registro não encontrado");
            return OkData(b);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Bairro dto)
        {
            _db.Bairros.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Bairro dto)
        {
            var b = _db.Bairros.Find(id);
            if (b == null) return NotFoundError("Registro não encontrado");
            b.Nome = dto.Nome ?? b.Nome;
            b.MunicipioId = dto.MunicipioId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var b = _db.Bairros.Find(id);
            if (b == null) return NotFoundError("Registro não encontrado");
            b.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
