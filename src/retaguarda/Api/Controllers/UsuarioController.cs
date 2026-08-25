using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public UsuarioController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "usuarios.visualizar")]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Usuarios.AsQueryable();
            query = query.Where(x => x.Ativo);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new { x.Id, x.Nome, x.Email }).ToList();
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
            // TODO: Refactor Create - Usuario no longer has Username or PessoaId
            // Implement proper user creation with Email-based authentication
            
            var u = new Usuario 
            { 
                Nome = dto.Nome ?? string.Empty, 
                SenhaHash = dto.SenhaHash ?? string.Empty, 
                Email = dto.Email 
            };
            _db.Usuarios.Add(u);
            _db.SaveChanges();

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
            // TODO: Refactor Update - Usuario no longer has Username or PessoaId
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Nome = dto.Nome ?? u.Nome;
            u.Email = dto.Email ?? u.Email;
            if (!string.IsNullOrWhiteSpace(dto.SenhaHash)) u.SenhaHash = dto.SenhaHash;

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
            public PessoaDto? Pessoa { get; set; }
            public long[]? SetorIds { get; set; }
            public long? PadraoSetorId { get; set; }
            public long[]? PerfilIds { get; set; }
        }
    }
}
