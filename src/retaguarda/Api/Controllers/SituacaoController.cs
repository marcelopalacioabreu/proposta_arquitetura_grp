using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/situacoes")]
    public class SituacaoController : BaseController
    {
        private readonly ISituacaoServico _servico;

        public SituacaoController(ISituacaoServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] PesquisaParametrosDto parametros, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null, [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null, [FromQuery] string? contexto = null)
        {
            parametros = NormalizarPesquisaParametros(parametros, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte);
            var (items, total) = _servico.ListarAsync(parametros).Result;
            
            // Filtrar por contexto se fornecido
            if (!string.IsNullOrWhiteSpace(contexto))
            {
                items = items.Where(x => x.Contexto == contexto).ToList();
                total = items.Count;
            }
            
            return OkList(items, total, parametros.Pagina, parametros.TamanhoPagina);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "catalogos.situacoes.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "catalogos.situacoes.editar")]
        public IActionResult Create([FromBody] SituacaoDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var o = _servico.CriarAsync(dto).Result;
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "catalogos.situacoes.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "catalogos.situacoes.editar")]
        public IActionResult Update(long id, [FromBody] SituacaoDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            return OkMessage("Atualizado");
        }
    }
}
