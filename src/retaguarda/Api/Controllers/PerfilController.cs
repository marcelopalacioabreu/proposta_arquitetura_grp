using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/perfis")]
    public class PerfilController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public PerfilController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "perfis.visualizar")]
        public IActionResult GetAll([FromQuery] string? nome, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var q = _db.Perfis.AsQueryable().Where(x => x.Ativo);
            if (!string.IsNullOrWhiteSpace(nome)) q = q.Where(x => x.Nome.Contains(nome));
            var total = q.Count();
            var items = q.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { x.Id, x.Nome, x.AdministradorDoSistema }).ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "perfis.visualizar")]
        public IActionResult Get(long id)
        {
            var p = _db.Perfis.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            // Debug log: record requested id and returned entity values to help trace mismatches
            try
            {
                System.Console.WriteLine($"PerfilController.Get requested id={id} -> returned Perfil.Id={p.Id} AdministradorDoSistema={p.AdministradorDoSistema}");
            }
            catch { /* ignore logging errors */ }
            var permissoes = _db.PerfilPermissoes.Where(x => x.PerfilId == id && x.Ativo).Select(x => x.Chave).ToList();
            return OkData(new { perfil = p, permissoes });
        }

        [HttpPost]
        [Authorize(Policy = "perfis.editar")]
        public IActionResult Create([FromBody] PerfilDto dto)
        {
            var perfil = new Perfil { Nome = dto.Nome ?? string.Empty, AdministradorDoSistema = dto.AdministradorDoSistema };
            _db.Perfis.Add(perfil);
            _db.SaveChanges();

            if (dto.Permissoes != null)
            {
                foreach (var chave in dto.Permissoes.Distinct())
                {
                    var pp = new PerfilPermissao { PerfilId = perfil.Id, Chave = chave };
                    _db.PerfilPermissoes.Add(pp);
                }
                _db.SaveChanges();
            }

            return CreatedDataAtAction(nameof(Get), new { id = perfil.Id }, perfil, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "perfis.editar")]
        public IActionResult Update(long id, [FromBody] PerfilDto dto)
        {
            var perfil = _db.Perfis.Find(id);
            if (perfil == null) return NotFoundError("Registro não encontrado");
            perfil.Nome = dto.Nome ?? perfil.Nome;
            perfil.AdministradorDoSistema = dto.AdministradorDoSistema;

            if (dto.Permissoes != null)
            {
                var existing = _db.PerfilPermissoes.Where(x => x.PerfilId == id).ToList();
                _db.PerfilPermissoes.RemoveRange(existing);
                _db.SaveChanges();
                foreach (var chave in dto.Permissoes.Distinct())
                {
                    var pp = new PerfilPermissao { PerfilId = id, Chave = chave };
                    _db.PerfilPermissoes.Add(pp);
                }
            }

            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "perfis.excluir")]
        public IActionResult Delete(long id)
        {
            var p = _db.Perfis.Find(id);
            if (p == null) return NotFoundError("Registro não encontrado");
            p.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }

        public class PerfilDto
        {
            public string? Nome { get; set; }
            public bool AdministradorDoSistema { get; set; }
            public string[]? Permissoes { get; set; }
        }
    }
}
