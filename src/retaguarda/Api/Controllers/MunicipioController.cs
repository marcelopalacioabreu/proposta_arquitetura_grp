using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/municipios")]
    public class MunicipioController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public MunicipioController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Municipios.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, UfId = x.UfId, UfNome = x.Uf != null ? x.Uf.Nome : string.Empty })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var m = _db.Municipios.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            return OkData(m);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Municipio dto)
        {
            _db.Municipios.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Municipio dto)
        {
            var m = _db.Municipios.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            m.Nome = dto.Nome ?? m.Nome;
            m.UfId = dto.UfId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var m = _db.Municipios.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            m.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
