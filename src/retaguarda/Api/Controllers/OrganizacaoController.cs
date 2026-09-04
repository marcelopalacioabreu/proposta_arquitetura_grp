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
    [Route("api/organizacoes")]
    public class OrganizacaoController : BaseController
    {
        private readonly IOrganizacaoServico _servico;
        private readonly IApplicationDbContext _db;

        public OrganizacaoController(IOrganizacaoServico servico, IApplicationDbContext db)
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
            e.Enderecos = EnderecoHelper.CarregarEnderecosOrganizacao(_db, id);
            return OkData(e);
        }

        [HttpPost]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Create([FromBody] OrganizacaoDto dto)
        {
            var o = _servico.CriarAsync(dto).Result;
            if (dto.Enderecos?.Length > 0) EnderecoHelper.SalvarEnderecosOrganizacao(_db, o.Id, dto.Enderecos);
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
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
            _servico.RestaurarAsync(id).GetAwaiter().GetResult();
            return OkMessage("Restaurado");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "organizacoes.editar")]
        public IActionResult Update(long id, [FromBody] OrganizacaoDto dto)
        {
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            if (dto.Enderecos != null) EnderecoHelper.SalvarEnderecosOrganizacao(_db, id, dto.Enderecos);
            return OkMessage("Atualizado");
        }
        
    }
}
