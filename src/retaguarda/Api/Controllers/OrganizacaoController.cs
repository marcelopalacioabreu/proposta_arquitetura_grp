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
        public IActionResult GetAll()
        {
            // For now return an empty list (placeholder - implement listing in service)
            return Ok(new object[] { });
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
            var o = new Organizacao { Nome = dto.Nome };
            // Direct DB access for simplicity
            // TODO: use service to create
            return CreatedAtAction(nameof(Get), new { id = 0 }, o);
        }

        public class OrganizacaoDto
        {
            public string Nome { get; set; } = string.Empty;
        }
    }
}
