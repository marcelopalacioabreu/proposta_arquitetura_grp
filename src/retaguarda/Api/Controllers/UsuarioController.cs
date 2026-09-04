using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Retaguarda.Persistencia;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Api.Models;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuarioController : BaseController
    {
        private readonly IApplicationDbContext _db;

        public UsuarioController(IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Policy = "usuarios.visualizar")]
        public IActionResult GetAll([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _db.Usuarios.AsQueryable().Where(x => x.Ativo);
            if (!string.IsNullOrWhiteSpace(q)) query = query.Where(x => x.Nome.Contains(q));
            var total = query.Count();
            var items = query.OrderBy(x => x.Nome).Skip((page - 1) * pageSize).Take(pageSize)
                .Select(x => new { x.Id, x.Nome, x.Email }).ToList();
            return OkList(items, total, page, pageSize);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "usuarios.visualizar")]
        public IActionResult Get(long id)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");

            var perfis = _db.PerfilUsuarios
                .Where(x => x.UsuarioId == id)
                .Select(x => new { id = x.Id, perfilId = x.PerfilId })
                .ToList<object>();

            var atuacoes = _db.SetorUsuarios
                .Where(x => x.UsuarioId == id)
                .Select(x => new { id = x.Id, setorId = x.SetorId, padrao = x.Padrao, habilitarPermissoesNegativas = x.HabilitarPermissoesNegativas })
                .ToList<object>();

            return OkData(new { u.Id, u.Nome, u.Email, perfis, atuacoes });
        }

        [HttpPost]
        [Authorize(Policy = "usuarios.editar")]
        public IActionResult Create([FromBody] UsuarioDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
                return BadRequest(EnvelopeResult.Error("Nome é obrigatório"));

            var u = new Usuario { Nome = dto.Nome, Email = dto.Email, SenhaHash = dto.SenhaHash ?? string.Empty };
            _db.Usuarios.Add(u);
            _db.SaveChanges();

            SalvarPerfis(u.Id, dto);
            SalvarAtuacoes(u.Id, dto);
            _db.SaveChanges();

            return CreatedDataAtAction(nameof(Get), new { id = u.Id }, new { u.Id, u.Nome, u.Email }, "Criado com sucesso");
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "usuarios.editar")]
        public IActionResult Update(long id, [FromBody] UsuarioDto dto)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");

            if (!string.IsNullOrWhiteSpace(dto.Nome)) u.Nome = dto.Nome;
            if (dto.Email != null) u.Email = dto.Email;
            if (!string.IsNullOrWhiteSpace(dto.SenhaHash)) u.SenhaHash = dto.SenhaHash;

            if (dto.Perfis != null || dto.PerfilIds != null)
            {
                _db.PerfilUsuarios.RemoveRange(_db.PerfilUsuarios.Where(x => x.UsuarioId == id));
                SalvarPerfis(id, dto);
            }

            if (dto.Atuacoes != null)
            {
                _db.SetorUsuarios.RemoveRange(_db.SetorUsuarios.Where(x => x.UsuarioId == id));
                SalvarAtuacoes(id, dto);
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

        [HttpPost("{id}/restaurar")]
        [Authorize(Policy = "usuarios.editar")]
        public IActionResult Restaurar(long id)
        {
            var u = _db.Usuarios.Find(id);
            if (u == null) return NotFoundError("Registro não encontrado");
            u.Ativo = true;
            _db.SaveChanges();
            return OkMessage("Restaurado");
        }

        private void SalvarPerfis(long usuarioId, UsuarioDto dto)
        {
            // Aceita perfis via subcadastro (Perfis) ou array legado (PerfilIds)
            var ids = dto.Perfis?.Where(p => p.PerfilId > 0).Select(p => p.PerfilId.Value)
                      ?? dto.PerfilIds ?? System.Linq.Enumerable.Empty<long>();
            foreach (var pid in ids.Distinct())
                _db.PerfilUsuarios.Add(new PerfilUsuario { UsuarioId = usuarioId, PerfilId = pid });
        }

        private void SalvarAtuacoes(long usuarioId, UsuarioDto dto)
        {
            if (dto.Atuacoes == null) return;
            foreach (var a in dto.Atuacoes.Where(a => a.SetorId > 0))
                _db.SetorUsuarios.Add(new SetorUsuario
                {
                    UsuarioId = usuarioId,
                    SetorId = a.SetorId ?? 0,
                    Padrao = a.Padrao,
                    HabilitarPermissoesNegativas = a.HabilitarPermissoesNegativas
                });
        }

        public class UsuarioDto
        {
            public string? Nome { get; set; }
            public string? SenhaHash { get; set; }
            public string? Email { get; set; }
            public PerfilContextoDto[]? Perfis { get; set; }
            public AtuacaoDto[]? Atuacoes { get; set; }
            // Compat com envios legados
            public long[]? PerfilIds { get; set; }

            public class PerfilContextoDto { public long? PerfilId { get; set; } }

            public class AtuacaoDto
            {
                public long? SetorId { get; set; }
                public bool Padrao { get; set; }
                public bool HabilitarPermissoesNegativas { get; set; }
            }
        }
    }
}
