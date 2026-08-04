using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/ceps")]
    public class CepController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public CepController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Ceps.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Codigo.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Codigo).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Codigo, Imovel = x.Imovel != null ? x.Imovel.Cadastro : string.Empty, x.ImovelId })
                .ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var c = _db.Ceps.Find(id);
            if (c == null) return NotFoundError("Registro não encontrado");
            return OkData(c);
        }

        [HttpGet("codigo/{codigo}")]
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

            var imovel = c.Imovel;
            var logradouro = imovel?.Logradouro;
            var bairro = logradouro?.Bairro;
            var municipio = bairro?.Municipio;
            var uf = municipio?.Uf;
            var pais = uf?.Pais;

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
        public IActionResult Create([FromBody] Cep dto)
        {
            _db.Ceps.Add(dto);
            _db.SaveChanges();
            return CreatedDataAtAction(nameof(Get), new { id = dto.Id }, dto, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Cep dto)
        {
            var c = _db.Ceps.Find(id);
            if (c == null) return NotFoundError("Registro não encontrado");
            c.Codigo = dto.Codigo ?? c.Codigo;
            c.ImovelId = dto.ImovelId;
            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var c = _db.Ceps.Find(id);
            if (c == null) return NotFoundError("Registro não encontrado");
            c.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }
    }
}
