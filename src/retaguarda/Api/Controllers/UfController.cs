using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/ufs")]
    public class UfController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public UfController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Ufs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q) || x.Sigla.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, x.Sigla, PaisNome = x.Pais != null ? x.Pais.Nome : string.Empty, x.PaisId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var u = _db.Ufs.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            return OkData(u);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Uf dto)
        {
            _db.Ufs.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Uf dto)
        {
            var u = _db.Ufs.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Nome = dto.Nome ?? u.Nome;
            u.Sigla = dto.Sigla ?? u.Sigla;
            u.PaisId = dto.PaisId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var u = _db.Ufs.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
