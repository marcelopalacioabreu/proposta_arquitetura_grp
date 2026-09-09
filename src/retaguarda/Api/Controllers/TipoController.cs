using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/tipos")]
    public class TipoController : BaseController
    {
        private readonly Retaguarda.Servicos.Interfaces.ITipoServico _servico;

        public TipoController(Retaguarda.Servicos.Interfaces.ITipoServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] string? contexto, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            (List<TipoDto> items, int total) result;
            
            // Se contexto for fornecido, usar o método de filtro específico
            if (!string.IsNullOrWhiteSpace(contexto))
            {
                result = await _servico.ListarPorContextoAsync(q, contexto, page, pageSize, null, null);
            }
            else
            {
                // Caso contrário, usar o ListarAsync padrão
                result = await _servico.ListarAsync(q, page, pageSize, null, null);
            }

            return OkList(result.items, result.total, page, pageSize);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _servico.ObterPorIdAsync(id);
            if (item == null) return NotFoundError("Tipo não encontrado");
            return OkData(item);
        }

        [HttpPost]
        [Authorize(Policy = "catalogos.tipos.editar")]
        public async Task<IActionResult> Create([FromBody] TipoDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest("Nome é obrigatório");
            if (string.IsNullOrWhiteSpace(dto.Contexto))
                return BadRequest("Contexto é obrigatório");

            var item = await _servico.CriarAsync(dto);
            return CreatedDataAtAction(nameof(Get), new { id = item.Id }, item, "Tipo criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "catalogos.tipos.editar")]
        public async Task<IActionResult> Update(long id, [FromBody] TipoDto dto)
        {
            var existing = await _servico.ObterPorIdAsync(id);
            if (existing == null) return NotFoundError("Tipo não encontrado");

            await _servico.UpdateAsync(id, dto);
            return OkMessage("Tipo atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "catalogos.tipos.excluir")]
        public async Task<IActionResult> Delete(long id)
        {
            var existing = await _servico.ObterPorIdAsync(id);
            if (existing == null) return NotFoundError("Tipo não encontrado");

            await _servico.DeleteAsync(id);
            return OkMessage("Tipo deletado");
        }
    }
}
