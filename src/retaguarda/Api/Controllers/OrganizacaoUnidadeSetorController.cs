using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Api.Models;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/organizacao_unidade_setores")]
    public class OrganizacaoUnidadeSetorController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public OrganizacaoUnidadeSetorController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] int? organizacaoUnidadeId = null, [FromQuery] int? inativo = null)
        {
            var q = _db.OrganizacaoUnidadeSetores.AsQueryable();
            if (inativo.HasValue && inativo.Value == 1) q = q.Where(x => !x.Ativo);
            else q = q.Where(x => x.Ativo);
            if (!string.IsNullOrEmpty(nome)) q = q.Where(x => x.Nome.Contains(nome));
            if (organizacaoUnidadeId.HasValue) q = q.Where(x => x.OrganizacaoUnidadeId == organizacaoUnidadeId.Value);

            var total = q.Count();
            if (!string.IsNullOrEmpty(sortField))
            {
                if (sortField == "nome") q = sortDir == "desc" ? q.OrderByDescending(x => x.Nome) : q.OrderBy(x => x.Nome);
            }
            q = q.Skip((page - 1) * pageSize).Take(pageSize);
            var items = q.Select(x => new { x.Id, x.Nome, x.OrganizacaoUnidadeId, x.DataInsercao }).ToList();
            return OkList(items, total, page, pageSize);
        }

        // Nested route for /api/organizacao_unidades/{organizacaoUnidadeId}/setores
        [HttpGet("~/api/organizacao_unidades/{organizacaoUnidadeId}/setores")]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult GetByUnidade(long organizacaoUnidadeId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            return GetAll(null, page, pageSize, null, null, (int)organizacaoUnidadeId, null);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _db.OrganizacaoUnidadeSetores.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Create([FromBody] OrganizacaoUnidadeSetorDto dto)
        {
            var s = new OrganizacaoUnidadeSetor { Nome = dto.Nome ?? string.Empty, OrganizacaoUnidadeId = dto.OrganizacaoUnidadeId };
            _db.OrganizacaoUnidadeSetores.Add(s);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = s.Id }, s, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Update(long id, [FromBody] OrganizacaoUnidadeSetorDto dto)
        {
            var existing = _db.OrganizacaoUnidadeSetores.Find(id);
            if (existing == null) return NotFoundError("Registro não encontrado");
            existing.Nome = dto.Nome ?? existing.Nome;
            existing.OrganizacaoUnidadeId = dto.OrganizacaoUnidadeId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "organizacoes.excluir")]
        public IActionResult Delete(long id)
        {
            var e = _db.OrganizacaoUnidadeSetores.Find(id);
            if (e == null) return NotFoundError("Registro não encontrado");
            _db.OrganizacaoUnidadeSetores.Remove(e);
            _db.SaveChanges();
            return OkMessage("Excluído");
        }

        public class OrganizacaoUnidadeSetorDto
        {
            public string? Nome { get; set; }
            public long? OrganizacaoUnidadeId { get; set; }
        }
    }
}
