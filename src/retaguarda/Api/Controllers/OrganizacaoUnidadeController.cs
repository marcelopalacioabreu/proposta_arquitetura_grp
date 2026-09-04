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
    [Route("api/organizacao_unidades")]
    public class OrganizacaoUnidadeController : BaseController
    {
        private readonly IOrganizacaoUnidadeServico _servico;
        private readonly IApplicationDbContext _db;

        public OrganizacaoUnidadeController(IOrganizacaoUnidadeServico servico, IApplicationDbContext db)
        {
            _servico = servico;
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult GetAll([FromQuery] PesquisaParametrosDto parametros, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null, [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null)
        {
            parametros = NormalizarPesquisaParametros(parametros, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte);
            var (items, total) = _servico.ListarAsync(parametros).Result;
            return OkList(items, total, parametros.Pagina, parametros.TamanhoPagina);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "organizacoes.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            e.Enderecos = EnderecoHelper.CarregarEnderecosUnidade(_db, id);
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Create([FromBody] OrganizacaoUnidadeDto dto)
        {
            var o = _servico.CriarAsync(dto).Result;
            if (dto.Enderecos?.Length > 0) EnderecoHelper.SalvarEnderecosUnidade(_db, o.Id, dto.Enderecos);
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Update(long id, [FromBody] OrganizacaoUnidadeDto dto)
        {
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            if (dto.Enderecos != null) EnderecoHelper.SalvarEnderecosUnidade(_db, id, dto.Enderecos);
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "organizacoes.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }

        [HttpPost("{id}/restaurar")]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Restaurar(long id)
        {
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.RestaurarAsync(id).GetAwaiter().GetResult();
            return OkMessage("Restaurado");
        }
    }
}
