using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/enderecos")]
    public class EnderecoController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public EnderecoController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Enderecos.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Complemento.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.CepId, x.Complemento, x.UsuarioId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var e = _db.Enderecos.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Endereco dto)
        {
            _db.Enderecos.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Endereco dto)
        {
            var e = _db.Enderecos.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            e.CepId = dto.CepId;
            e.Complemento = dto.Complemento;
            e.UsuarioId = dto.UsuarioId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var e = _db.Enderecos.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            e.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
