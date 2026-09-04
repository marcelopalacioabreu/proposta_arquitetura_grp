using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Parametros;
using Retaguarda.Persistencia;
using Retaguarda.Api.Utils;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/pessoas")]
    public class PessoaController : BaseController
    {
        private readonly IPessoaServico _servico;
        private readonly IApplicationDbContext _db;

        public PessoaController(IPessoaServico servico, IApplicationDbContext db)
        {
            _servico = servico;
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "pessoas.visualizar")]
        public IActionResult GetAll([FromQuery] PesquisaParametrosDto parametros, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null, [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null)
        {
            parametros = NormalizarPesquisaParametros(parametros, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte);
            var (items, total) = _servico.ListarAsync(parametros).Result;
            return OkList(items, total, parametros.Pagina, parametros.TamanhoPagina);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "pessoas.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            e.Enderecos = EnderecoHelper.CarregarEnderecosPessoa(_db, id);
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "pessoas.editar")]
        public IActionResult Create([FromBody] PessoaDto dto)
        {
            var o = _servico.CriarAsync(dto).Result;
            if (dto.Enderecos?.Length > 0) EnderecoHelper.SalvarEnderecosPessoa(_db, o.Id, dto.Enderecos);
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "pessoas.editar")]
        public IActionResult Update(long id, [FromBody] PessoaDto dto)
        {
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            if (dto.Enderecos != null) EnderecoHelper.SalvarEnderecosPessoa(_db, id, dto.Enderecos);
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "pessoas.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }

        [HttpPost("{id}/restaurar")]
        [Authorize(Policy = "pessoas.editar")]
        public IActionResult Restaurar(long id)
        {
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.RestaurarAsync(id).GetAwaiter().GetResult();
            return OkMessage("Restaurado");
        }
    }
}