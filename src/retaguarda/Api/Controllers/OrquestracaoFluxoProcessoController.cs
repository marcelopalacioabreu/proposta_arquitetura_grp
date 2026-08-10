using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/orquestracaoFluxo/processos")]
    public class OrquestracaoFluxoProcessoController : BaseController
    {
        private readonly IOrquestracaoFluxoProcessoServico _servico;

        public OrquestracaoFluxoProcessoController(IOrquestracaoFluxoProcessoServico servico) => _servico = servico;

        [HttpGet]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null, [FromQuery] int? inativo = null)
        {
            var (items, total) = _servico.ListarAsync(nome, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte, inativo).Result;
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "orquestracaoFluxo.editar")]
        public IActionResult Create([FromBody] CriarDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var o = _servico.CriarAsync(dto.Nome, dto.Descricao, dto.WorkflowDefinitionId, dto.WorkflowVersion).Result;
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "orquestracaoFluxo.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }

        [HttpPost("{id}/restaurar")]
        [Authorize(Policy = "orquestracaoFluxo.editar")]
        public IActionResult Restaurar(long id)
        {
            _servico.RestaurarAsync(id).GetAwaiter().GetResult();
            return OkMessage("Restaurado");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "orquestracaoFluxo.editar")]
        public IActionResult Update(long id, [FromBody] AtualizarDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            existing.Nome = dto.Nome;
            existing.Descricao = dto.Descricao ?? string.Empty;
            existing.WorkflowDefinitionId = dto.WorkflowDefinitionId;
            existing.WorkflowVersion = dto.WorkflowVersion;
            _servico.UpdateAsync(existing).GetAwaiter().GetResult();
            return OkMessage("Atualizado");
        }

        public class CriarDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; }
            public string? WorkflowDefinitionId { get; set; }
            public int? WorkflowVersion { get; set; }
        }

        public class AtualizarDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; }
            public string? WorkflowDefinitionId { get; set; }
            public int? WorkflowVersion { get; set; }
        }
    }
}
