using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia.MYSQL;
using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public UsuarioController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "usuarios.visualizar")]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Usuarios.AsQueryable();
            query = query.Where(x => x.Ativo);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q) || x.Username.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { x.Id, x.Nome, x.Username, x.Email, x.PessoaId }).ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "usuarios.visualizar")]
        public IActionResult Get(long id)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            return OkData(u);
        }

        [HttpPost]
        [Authorize(Policy = "usuarios.editar")]
        public IActionResult Create([FromBody] UsuarioDto dto)
        {
            long? pessoaId = dto.PessoaId;
            if (dto.Pessoa != null)
            {
                var p = new Pessoa { Nome = dto.Pessoa.Nome ?? string.Empty, TipoPessoaChave = dto.Pessoa.TipoPessoaChave ?? "F", Documento = dto.Pessoa.Documento, Email = dto.Pessoa.Email, Telefone = dto.Pessoa.Telefone };
                _db.Pessoas.Add(p);
                _db.SaveChanges();
                pessoaId = p.Id;
            }
            var u = new Usuario { Nome = dto.Nome ?? string.Empty, Username = dto.Username ?? string.Empty, SenhaHash = dto.SenhaHash ?? string.Empty, Email = dto.Email, PessoaId = pessoaId, OrganizacaoId = dto.OrganizacaoId };
            _db.Usuarios.Add(u);
            _db.SaveChanges();

            // Associate provided setorIds if any
            if (dto.SetorIds != null && dto.SetorIds.Any())
            {
                foreach (var s in dto.SetorIds)
                {
                    var su = new SetorUsuario { UsuarioId = u.Id, SetorId = s, Padrao = (dto.PadraoSetorId.HasValue && dto.PadraoSetorId.Value == s) };
                    _db.SetorUsuarios.Add(su);
                }
                _db.SaveChanges();
            }

            // Associate provided perfilIds if any
            if (dto.PerfilIds != null && dto.PerfilIds.Any())
            {
                foreach (var p in dto.PerfilIds)
                {
                    var pu = new PerfilUsuario { UsuarioId = u.Id, PerfilId = p };
                    _db.PerfilUsuarios.Add(pu);
                }
                _db.SaveChanges();
            }

            return CreatedDataAtAction(nameof(Get), new { id = u.Id }, u, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "usuarios.editar")]
        public IActionResult Update(long id, [FromBody] UsuarioDto dto)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Nome = dto.Nome ?? u.Nome;
            u.Email = dto.Email ?? u.Email;
            u.Username = dto.Username ?? u.Username;
            if (!string.IsNullOrWhiteSpace(dto.SenhaHash)) u.SenhaHash = dto.SenhaHash;
            if (dto.Pessoa != null)
            {
                if (u.PessoaId.HasValue)
                {
                    var p = _db.Pessoas.Find(u.PessoaId.Value);
                    if (p != null)
                    {
                        p.Nome = dto.Pessoa.Nome ?? p.Nome;
                        p.Documento = dto.Pessoa.Documento ?? p.Documento;
                        p.Email = dto.Pessoa.Email ?? p.Email;
                        p.Telefone = dto.Pessoa.Telefone ?? p.Telefone;
                    }
                }
                else
                {
                    var p = new Pessoa { Nome = dto.Pessoa.Nome ?? string.Empty, TipoPessoaChave = dto.Pessoa.TipoPessoaChave ?? "F", Documento = dto.Pessoa.Documento, Email = dto.Pessoa.Email, Telefone = dto.Pessoa.Telefone };
                    _db.Pessoas.Add(p);
                    _db.SaveChanges();
                    u.PessoaId = p.Id;
                }
            }
            // update setor associations if provided
            if (dto.SetorIds != null)
            {
                var existing = _db.SetorUsuarios.Where(x => x.UsuarioId == u.Id).ToList();
                _db.SetorUsuarios.RemoveRange(existing);
                _db.SaveChanges();
                foreach (var s in dto.SetorIds)
                {
                    var su = new SetorUsuario { UsuarioId = u.Id, SetorId = s, Padrao = (dto.PadraoSetorId.HasValue && dto.PadraoSetorId.Value == s) };
                    _db.SetorUsuarios.Add(su);
                }
            }

            // update perfil associations if provided
            if (dto.PerfilIds != null)
            {
                var existingPerfis = _db.PerfilUsuarios.Where(x => x.UsuarioId == u.Id).ToList();
                _db.PerfilUsuarios.RemoveRange(existingPerfis);
                _db.SaveChanges();
                foreach (var p in dto.PerfilIds)
                {
                    var pu = new PerfilUsuario { UsuarioId = u.Id, PerfilId = p };
                    _db.PerfilUsuarios.Add(pu);
                }
            }

            _db.SaveChanges();
            return OkMessage("Atualizado");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "usuarios.excluir")]
        public IActionResult Delete(long id)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Ativo = false;
            _db.SaveChanges();
            return OkMessage("Excluído");
        }

        public class UsuarioDto
        {
            public string? Nome { get; set; }
            public string? Username { get; set; }
            public string? SenhaHash { get; set; }
            public string? Email { get; set; }
            public long? OrganizacaoId { get; set; }
            public long? PessoaId { get; set; }
            public PessoaController.PessoaDto? Pessoa { get; set; }
            public long[]? SetorIds { get; set; }
            public long? PadraoSetorId { get; set; }
            public long[]? PerfilIds { get; set; }
        }
    }
}
