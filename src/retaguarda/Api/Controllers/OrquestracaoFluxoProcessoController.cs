using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.Api.Utils;
using Retaguarda.Servicos;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/orquestracaoFluxo/processos")]
    public class OrquestracaoFluxoProcessoController : BaseController
    {
        private readonly IOrquestracaoFluxoProcessoServico _servico;
        private readonly EscopoEmExecucao _escopo;
        private readonly HttpClient _httpClient;

        public OrquestracaoFluxoProcessoController(IOrquestracaoFluxoProcessoServico servico, EscopoEmExecucao escopo, HttpClient httpClient) 
        { 
            _servico = servico;
            _escopo = escopo;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null,
            [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null, [FromQuery] int? inativo = null)
        {
            var filtros = FiltrosHelper.MontarFiltros(campo, operador, valor, valorDe, valorAte);
            var parametros = new Retaguarda.DTO.Parametros.PesquisaParametrosDto
            {
                Nome = nome,
                Pagina = page,
                TamanhoPagina = pageSize,
                SortField = sortField,
                SortDir = sortDir,
                Filtros = filtros,
                Inativo = inativo
            };

            var (items, total) = _servico.ListarAsync(parametros).Result;
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public IActionResult Get(long id)
        {
            var dto = _servico.ObterPorIdAsync(id).Result;
            if (dto == null) return NotFoundError("Registro não encontrado");
            return OkData(dto);
        }

        [HttpGet("contexto/atual")]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public IActionResult ObterContextoAtual()
        {
            return OkData(new 
            { 
                organizacaoId = _escopo.OrganizacaoId,
                organizacaoUnidadeId = _escopo.OrganizacaoUnidadeId,
                setorId = _escopo.SetorId
            });
        }

        [HttpGet("workflows")]
        [Authorize(Policy = "orquestracaoFluxo.visualizar")]
        public async Task<IActionResult> ListarWorkflows()
        {
            try
            {
                // Chamar endpoint real do PlanejadorFluxo via reverse proxy
                var response = await _httpClient.GetAsync("http://localhost:6001/elsa/api/workflow-definitions");
                
                if (!response.IsSuccessStatusCode)
                {
                    // Fallback com dados mock se serviço estiver indisponível
                    var mockWorkflows = new List<object>
                    {
                        new { id = "workflow-1", definitionId = "workflow-1", name = "Workflow 1 - Processo Padrão", description = "Processo padrão", version = 1 },
                        new { id = "workflow-2", definitionId = "workflow-2", name = "Workflow 2 - Aprovação Hierárquica", description = "Com aprovação em cadeia", version = 1 },
                        new { id = "workflow-3", definitionId = "workflow-3", name = "Workflow 3 - Notificação", description = "Apenas notificação", version = 1 }
                    };
                    return OkData(mockWorkflows);
                }

                var content = await response.Content.ReadAsStringAsync();
                return Ok(content);
            }
            catch
            {
                // Fallback com dados mock em caso de exceção
                var mockWorkflows = new List<object>
                {
                    new { id = "workflow-1", definitionId = "workflow-1", name = "Workflow 1 - Processo Padrão", description = "Processo padrão", version = 1 },
                    new { id = "workflow-2", definitionId = "workflow-2", name = "Workflow 2 - Aprovação Hierárquica", description = "Com aprovação em cadeia", version = 1 },
                    new { id = "workflow-3", definitionId = "workflow-3", name = "Workflow 3 - Notificação", description = "Apenas notificação", version = 1 }
                };
                return OkData(mockWorkflows);
            }
        }

        [HttpPost]
        [Authorize(Policy = "orquestracaoFluxo.editar")]
        public IActionResult Create([FromBody] CriarDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var toCreate = new OrquestracaoFluxoProcessoDto 
            { 
                Nome = dto.Nome, 
                Descricao = dto.Descricao, 
                WorkflowDefinitionId = dto.WorkflowDefinitionId, 
                WorkflowVersion = dto.WorkflowVersion,
                WorkflowJson = dto.WorkflowJson,
                WorkflowNome = dto.WorkflowNome
            };
            var o = _servico.CriarAsync(toCreate).Result;
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
            var toUpdate = new OrquestracaoFluxoProcessoDto 
            { 
                Nome = dto.Nome, 
                Descricao = dto.Descricao, 
                WorkflowDefinitionId = dto.WorkflowDefinitionId, 
                WorkflowVersion = dto.WorkflowVersion,
                WorkflowJson = dto.WorkflowJson,
                WorkflowNome = dto.WorkflowNome
            };
            _servico.UpdateAsync(id, toUpdate).GetAwaiter().GetResult();
            return OkMessage("Atualizado");
        }

        public class CriarDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; }
            public string? WorkflowDefinitionId { get; set; }
            public int? WorkflowVersion { get; set; }
            public string? WorkflowJson { get; set; }
            public string? WorkflowNome { get; set; }
        }

        public class AtualizarDto
        {
            [System.ComponentModel.DataAnnotations.Required]
            public string Nome { get; set; } = string.Empty;
            public string? Descricao { get; set; }
            public string? WorkflowDefinitionId { get; set; }
            public int? WorkflowVersion { get; set; }
            public string? WorkflowJson { get; set; }
            public string? WorkflowNome { get; set; }
        }
    }
}
