using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Api.Models;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/organizacao_unidades")]
    public class OrganizacaoUnidadeController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public OrganizacaoUnidadeController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] int? organizacaoId = null, [FromQuery] int? inativo = null)
        {
            var q = _db.OrganizacaoUnidades.AsQueryable();
            if (inativo.HasValue && inativo.Value == 1) q = q.Where(x => !x.Ativo);
            else q = q.Where(x => x.Ativo);
            if (!string.IsNullOrEmpty(nome)) q = q.Where(x => x.Nome.Contains(nome));
            if (organizacaoId.HasValue) q = q.Where(x => x.OrganizacaoId == organizacaoId.Value);

            var total = q.Count();
            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome);
            }
            q = q.Skip((page - 1) * pageSize).Take(pageSize);
            var items = q.Select(x => new { x.Id, x.Nome, x.OrganizacaoId, x.DataInsercao }).ToList();
            return OkList(items, total, page, pageSize);
        }

        // Nested route for /api/organizacoes/{organizacaoId}/unidades
        [HttpGet("~/api/organizacoes/{organizacaoId}/unidades")]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult GetByOrganizacao(long organizacaoId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return GetAll(null, page, pageSize, null, null, (int)organizacaoId, null);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _db.OrganizacaoUnidades.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Create([FromBody] OrganizacaoUnidadeDto dto)
        {
            var s = new OrganizacaoUnidade { Nome = dto.Nome ?? string.Empty, OrganizacaoId = dto.OrganizacaoId };
            _db.OrganizacaoUnidades.Add(s);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = s.Id }, s, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Update(long id, [FromBody] OrganizacaoUnidadeDto dto)
        {
            var existing = _db.OrganizacaoUnidades.Find(id);
            if (existing == null) return NotFoundError("Registro não encontrado");
            existing.Nome = dto.Nome ?? existing.Nome;
            existing.OrganizacaoId = dto.OrganizacaoId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "organizacoes.excluir")]
        public IActionResult Delete(long id)
        {
            var e = _db.OrganizacaoUnidades.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            _db.OrganizacaoUnidades.Remove(e);
            _db.SaveChanges();
            return OkMessage("Excluído");
        }

        public class OrganizacaoUnidadeDto
        {
            public string? Nome { get; set; }
            public long? OrganizacaoId { get; set; }
        }
    }
}
