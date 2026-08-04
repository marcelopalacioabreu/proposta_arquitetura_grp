using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/pessoas")]
    public class PessoaController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public PessoaController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "pessoas.visualizar")]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Pessoas.AsQueryable();
            query = query.Where(x => x.Ativo);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q) || (x.Documento != null && x.Documento.Contains(q)));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { x.Id, x.Nome, x.TipoPessoaChave, x.Documento }).ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "pessoas.visualizar")]
        public IActionResult Get(long id)
        {
            var p = _db.Pessoas.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            return OkData(p);
        }

        [HttpPost]
        [Authorize(Policy = "pessoas.editar")]
        public IActionResult Create([FromBody] PessoaDto dto)
        {
            var p = new Pessoa { Nome = dto.Nome ?? string.Empty, TipoPessoaChave = dto.TipoPessoaChave ?? "F", Documento = dto.Documento, Email = dto.Email, Telefone = dto.Telefone };
            _db.Pessoas.Add(p);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = p.Id }, p, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "pessoas.editar")]
        public IActionResult Update(long id, [FromBody] PessoaDto dto)
        {
            var p = _db.Pessoas.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            p.Nome = dto.Nome ?? p.Nome;
            p.TipoPessoaChave = dto.TipoPessoaChave ?? p.TipoPessoaChave;
            p.Documento = dto.Documento ?? p.Documento;
            p.Email = dto.Email ?? p.Email;
            p.Telefone = dto.Telefone ?? p.Telefone;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "pessoas.excluir")]
        public IActionResult Delete(long id)
        {
            var p = _db.Pessoas.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            p.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }

        public class PessoaDto
        {
            public string? Nome { get; set; }
            public string? TipoPessoaChave { get; set; }
            public string? Documento { get; set; }
            public string? Telefone { get; set; }
            public string? Email { get; set; }
        }
    }
}
