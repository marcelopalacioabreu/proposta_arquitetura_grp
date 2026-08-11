using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Parametros;
using Retaguarda.Persistencia;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/ceps")]
    public class CepController : BaseController
    {
        private readonly ICepServico _servico;
        private readonly IApplicationDbContext _db;

        public CepController(ICepServico servico, IApplicationDbContext db)
        {
            _servico = servico;
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "ceps.visualizar")]
        public IActionResult GetAll([FromQuery] PesquisaParametrosDto parametros, [FromQuery] int? page = null, [FromQuery] int? pageSize = null, [FromQuery] string? sortField = null, [FromQuery] string? sortDir = null, [FromQuery] string? campo = null, [FromQuery] string? operador = null, [FromQuery] string? valor = null, [FromQuery(Name = "valor_de")] string? valorDe = null, [FromQuery(Name = "valor_ate")] string? valorAte = null)
        {
            parametros = NormalizarPesquisaParametros(parametros, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte);
            var (items, total) = _servico.ListarAsync(parametros).Result;
            return OkList(items, total, parametros.Pagina, parametros.TamanhoPagina);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "ceps.visualizar")]
        public IActionResult Get(long id)
        {
            var e = _servico.ObterPorIdAsync(id).Result;
            if (e == null) return NotFoundError("Registro não encontrado");
            return OkData(e);
        }

        [HttpGet("codigo/{codigo}")]
        [Authorize(Policy = "ceps.visualizar")]
        public IActionResult GetByCodigo(string codigo)
        {
            var c = _db.Ceps
                .Include(x => x.Imovel)                
                    .ThenInclude(i => i.Logradouro)
                        .ThenInclude(l => l.Bairro)
                            .ThenInclude(b => b.Municipio)
                                .ThenInclude(m => m.Uf)
                                    .ThenInclude(u => u.Pais)
                .FirstOrDefault(x => x.Codigo == codigo);

            if (c == null) return NotFoundError("CEP não encontrado");

            var imovel = c.Imovel!;
            var logradouro = imovel?.Logradouro!;
            var bairro = logradouro?.Bairro!;
            var municipio = bairro?.Municipio!;
            var uf = municipio?.Uf!;
            var pais = uf?.Pais!;

            var result = new
            {
                c.Id,
                c.Codigo,
                Imovel = imovel == null ? null : new
                {
                    imovel.Id,
                    imovel.Cadastro,
                    Logradouro = logradouro == null ? null : new
                    {
                        logradouro.Id,
                        logradouro.Tipo,
                        logradouro.Nome,
                        Bairro = bairro == null ? null : new
                        {
                            bairro.Id,
                            bairro.Nome,
                            Municipio = municipio == null ? null : new
                            {
                                municipio.Id,
                                municipio.Nome,
                                municipio.CodigoIbge,
                                Uf = uf == null ? null : new
                                {
                                    uf.Id,
                                    uf.Nome,
                                    uf.Sigla,
                                    Pais = pais == null ? null : new { pais.Id, pais.Nome }
                                }
                            }
                        }
                    }
                }
            };

            return OkData(result);
        }

        [HttpPost]
        [Authorize(Policy = "ceps.editar")]
        public IActionResult Create([FromBody] CepDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var o = _servico.CriarAsync(dto).Result;
            return CreatedDataAtAction(nameof(Get), new { id = o.Id }, o, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "ceps.editar")]
        public IActionResult Update(long id, [FromBody] CepDto dto)
        {
            if (!ModelState.IsValid) return BadRequestModelState();
            var existing = _servico.ObterPorIdAsync(id).Result;
            if (existing == null) return NotFoundError("Registro não encontrado");
            _servico.UpdateAsync(id, dto).GetAwaiter().GetResult();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "ceps.excluir")]
        public IActionResult Delete(long id)
        {
            _servico.DeleteAsync(id).GetAwaiter().GetResult();
            return OkMessage("Excluído");
        }
    }
}
