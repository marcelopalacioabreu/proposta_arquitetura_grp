using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Parametros;
using Retaguarda.Api.Utils;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/paises")]
    public class PaisController : BaseController
    {
        private readonly IPaisServico _servico;

        public PaisController(IPaisServico servico)
        {
            _servico = servico;
        }

        [HttpGet]
        [Authorize(Policy = "enderecos.paises.visualizar")]
        public IActionResult GetAll([FromQuery] PesquisaParametrosDto parametros, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null, [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null)
        {
            parametros = NormalizarPesquisaParametros(parametros, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte);
            var (items, total) = _servico.ListarAsync(parametros).Result;
            return OkList(items, total, parametros.Pagina, parametros.TamanhoPagina);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "enderecos.paises.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "enderecos.paises.editar")]
        public IActionResult Create([FromBody] PaisDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var o = _servico.CriarAsync(dto).Result;
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "enderecos.paises.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "enderecos.paises.editar")]
        public IActionResult Update(long id, [FromBody] PaisDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            return OkMessage("Atualizado");
        }
    }
}
