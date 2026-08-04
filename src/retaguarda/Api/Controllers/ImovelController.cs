using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/imoveis")]
    public class ImovelController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public ImovelController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Imoveis.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Cadastro.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Cadastro).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Cadastro, Logradouro = x.Logradouro != null ? x.Logradouro.Nome : string.Empty, x.LogradouroId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var m = _db.Imoveis.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            return OkData(m);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Imovel dto)
        {
            _db.Imoveis.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Imovel dto)
        {
            var m = _db.Imoveis.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            m.Cadastro = dto.Cadastro ?? m.Cadastro;
            m.LogradouroId = dto.LogradouroId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var m = _db.Imoveis.Find(id);
            if (m == null) return NotFoundError("Registro não encontrado");
            m.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
