using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/organizacoes")]
    public class OrganizacaoController : ControllerBase
    {
        private readonly IOrganizacaoServico _servico;

        public OrganizacaoController(IOrganizacaoServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null, [FromQuery] int? inativo = null)
        {
            var (items, total) = _servico.ListarAsync(nome, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte, inativo).Result;
            return Ok(new { items, total, page, pageSize });
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFound();
            return Ok(e);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] OrganizacaoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var o = _servico.CriarAsync(dto.Nome).Result;
            return CreatedAtAction(nameof(Get), new { id = o.Id }, o);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return NoContent();
        }

        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update(long id, [FromBody] OrganizacaoDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFound();
            existing.Nome = dto.Nome;
            _servico.UpdateAsync(existing).GetAwaiter().GetResult();
            return NoContent();
        }

        public class OrganizacaoDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Nome { get; set; } = string.Empty;
        }
    }
}
