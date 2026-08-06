using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/tiposimovel")]
    public class TipoImovelController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public TipoImovelController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.TiposImovel.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q) || x.Codigo.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { x.Id, x.Codigo, x.Nome }).ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var e = _db.TiposImovel.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TipoImovel dto)
        {
            _db.TiposImovel.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TipoImovel dto)
        {
            var e = _db.TiposImovel.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            e.Codigo = dto.Codigo ?? e.Codigo;
            e.Nome = dto.Nome ?? e.Nome;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var e = _db.TiposImovel.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            e.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
