using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/pessoas-juridicas")]
    public class PessoaJuridicaController : BaseController
    {
        private readonly Retaguarda.Servicos.Interfaces.IPessoaJuridicaServico _servico;

        public PessoaJuridicaController(Retaguarda.Servicos.Interfaces.IPessoaJuridicaServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total) = await _servico.ListarAsync(q, page, pageSize, null, null);
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(long id)
        {
            var item = await _servico.ObterPorIdAsync(id);
            if (item == null) return NotFoundError("Registro não encontrado");
            return OkData(item);
        }

        [HttpPost]
        [Authorize(Policy = "pessoas.editar")]
        public async Task<IActionResult> Create([FromBody] PessoaJuridicaDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RazaoSocial))
                return BadRequest("Razão Social é obrigatória");

            var item = await _servico.CriarAsync(dto);
            return CreatedDataAtAction(nameof(Get), new { id = item.Id }, item, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "pessoas.editar")]
        public async Task<IActionResult> Update(long id, [FromBody] PessoaJuridicaDto dto)
        {
            var existing = await _servico.ObterPorIdAsync(id);
            if (existing == null) return NotFoundError("Registro não encontrado");

            await _servico.UpdateAsync(id, dto);
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "pessoas.excluir")]
        public async Task<IActionResult> Delete(long id)
        {
            var existing = await _servico.ObterPorIdAsync(id);
            if (existing == null) return NotFoundError("Registro não encontrado");

            await _servico.DeleteAsync(id);
            return OkMessage("Deletado");
        }
    }
}
