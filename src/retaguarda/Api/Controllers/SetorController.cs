using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/setores")]
    public class SetorController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public SetorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null, [FromQuery] int? inativo = null)
        {
            var q = _db.OrganizacaoSetores.AsQueryable();
            // ativo / inativo default: ativos
            if (inativo.HasValue && inativo.Value == 1) q = q.Where(x => !x.Ativo);
            else q = q.Where(x => x.Ativo);
            if (!string.IsNullOrEmpty(nome)) q = q.Where(x => x.Nome.Contains(nome));

            // simple campo/operator handling for nome
            if (!string.IsNullOrWhiteSpace(campo) && !string.IsNullOrWhiteSpace(operador) && !string.IsNullOrWhiteSpace(valor))
            {
                var v = valor;
                if (campo == "nome")
                {
                    if (operador == "iniciando_com") q = q.Where(o => o.Nome.StartsWith(v));
                    else if (operador == "contendo") q = q.Where(o => o.Nome.Contains(v));
                    else if (operador == "terminando_com") q = q.Where(o => o.Nome.EndsWith(v));
                    else if (operador == "igual") q = q.Where(o => o.Nome == v);
                }
            }
            var total = q.Count();
            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome);
            }
            q = q.Skip((page - 1) * pageSize).Take(pageSize);
            var items = q.Select(x => new { x.Id, x.Nome, x.OrganizacaoId, x.DataInsercao }).ToList();
            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var e = _db.OrganizacaoSetores.Find(id);
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] SetorDto dto)
        {
            var s = new OrganizacaoSetor { Nome = dto.Nome ?? string.Empty, OrganizacaoId = dto.OrganizacaoId };
            _db.OrganizacaoSetores.Add(s);
            _db.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = s.Id }, s);
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(long id, [FromBody] SetorDto dto)
        {
            var existing = _db.OrganizacaoSetores.Find(id);
            if (existing == null) return NotFound();
            existing.Nome = dto.Nome ?? existing.Nome;
            existing.OrganizacaoId = dto.OrganizacaoId;
            _db.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(long id)
        {
            var e = _db.OrganizacaoSetores.Find(id);
            if (e == null) return NotFound();
            _db.OrganizacaoSetores.Remove(e);
            _db.SaveChanges();
            return NoContent();
        }

        public class SetorDto
        {
            public string? Nome { get; set; }
            public long? OrganizacaoId { get; set; }
        }
    }
}
