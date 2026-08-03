using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Retaguarda.Servicos;
using Retaguarda.Persistencia.MYSQL;
using Microsoft.EntityFrameworkCore;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/usuario/contexto")]
    public class UsuarioContextoController : BaseController
    {
        private readonly RequisicaoUsuario _reqUsuario;
        private readonly Retaguarda.Servicos.Interfaces.IUsuarioServico _usuarioServico;
        private readonly ApplicationDbContext _db;

        public UsuarioContextoController(RequisicaoUsuario reqUsuario, Retaguarda.Servicos.Interfaces.IUsuarioServico usuarioServico, ApplicationDbContext db)
        {
            _reqUsuario = reqUsuario;
            _usuarioServico = usuarioServico;
            _db = db;
        }

        public class ContextoRequest
        {
            public long? OrganizacaoId { get; set; }
            public long? OrganizacaoUnidadeId { get; set; }
            public long? SetorId { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Set([FromBody] ContextoRequest req)
        {
            var u = _reqUsuario.Usuario;
            if (u == null) return UnauthorizedError("Usuário não autenticado");
            // All three fields must be provided
            if (!req.OrganizacaoId.HasValue || !req.OrganizacaoUnidadeId.HasValue || !req.SetorId.HasValue)
                return BadRequest(new { message = "Selecione organização, unidade e setor válidos." });

            // Validate existence of entities
            var orgExists = await _db.Organizacoes.AnyAsync(o => o.Id == req.OrganizacaoId.Value && o.Ativo);
            var unidadeExists = await _db.OrganizacaoUnidades.AnyAsync(u2 => u2.Id == req.OrganizacaoUnidadeId.Value && u2.Ativo);
            var setorEntity = await _db.OrganizacaoSetores.FirstOrDefaultAsync(s => s.Id == req.SetorId.Value && s.Ativo);
            if (!orgExists || !unidadeExists || setorEntity == null)
                return BadRequest(new { message = "Um ou mais registros selecionados são inválidos." });

            // Validate that the selected setor matches the provided org/unidade
            if (setorEntity.OrganizacaoId != req.OrganizacaoId.Value || setorEntity.OrganizacaoUnidadeId != req.OrganizacaoUnidadeId.Value)
                return BadRequest(new { message = "O setor selecionado não pertence à organização/unidade informada." });

            // Validate access: admins can set any context
            var isAdmin = await _db.Perfis
                .Where(p => p.AdministradorDoSistema)
                .Join(_db.PerfilUsuarios, p => p.Id, pu => pu.PerfilId, (p, pu) => pu)
                .AnyAsync(pu => pu.UsuarioId == u.Id);

            if (!isAdmin)
            {
                var allowed = await _db.SetorUsuarios.AnyAsync(su => su.UsuarioId == u.Id && su.SetorId == req.SetorId.Value && su.Ativo);
                if (!allowed) return UnauthorizedError("Usuário não tem permissão para atuar nesse setor");
            }

            var updated = await _usuarioServico.AtualizarUltimoAcessoAsync(u.Id, req.OrganizacaoId, req.OrganizacaoUnidadeId, req.SetorId);
            if (updated == null) return Error("Falha ao atualizar usuário");

            // set cookie for atuacao so browser clients pick it up (HttpOnly)
            var cookieVal = System.Text.Json.JsonSerializer.Serialize(new { organizacaoId = req.OrganizacaoId, organizacaoUnidadeId = req.OrganizacaoUnidadeId, setorId = req.SetorId });
            Response.Cookies.Append("atuacao", cookieVal, new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.Lax });

            return OkMessage("Contexto atualizado");
        }
        
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var u = _reqUsuario.Usuario;

            // If no authenticated user, return a safe empty payload (200) so frontend can call without handling 401.
            if (u == null)
            {
                return OkData(new { administrado = false, organizacoes = new object[0], unidades = new object[0], setores = new object[0], ultimoAcesso = new { } });
            }

            // Check admin
            var isAdmin = await _db.Perfis
                .Where(p => p.AdministradorDoSistema)
                .Join(_db.PerfilUsuarios, p => p.Id, pu => pu.PerfilId, (p, pu) => pu)
                .AnyAsync(pu => pu.UsuarioId == u.Id);

            if (isAdmin)
            {
                // return full tree: organizacoes -> unidades -> setores
                var orgsAll = await _db.Organizacoes.Select(o => new { id = o.Id, nome = o.Nome }).ToListAsync();
                var unidadesAll = await _db.OrganizacaoUnidades.Select(u2 => new { id = u2.Id, nome = u2.Nome, organizacaoId = u2.OrganizacaoId }).ToListAsync();
                var setoresAll = await _db.OrganizacaoSetores.Select(s => new { id = s.Id, nome = s.Nome, organizacaoId = s.OrganizacaoId, organizacaoUnidadeId = s.OrganizacaoUnidadeId }).ToListAsync();
                return OkData(new { administrado = true, organizacoes = orgsAll, unidades = unidadesAll, setores = setoresAll, ultimoAcesso = new { organizacaoId = u.UltimoAcessoOrganizacaoId, organizacaoUnidadeId = u.UltimoAcessoOrganizacaoUnidadeId, setorId = u.UltimoAcessoSetorId } });
            }

            // Non-admin: return sectors linked to user
            var setoresUsuario = await _db.SetorUsuarios.Where(su => su.UsuarioId == u.Id && su.Ativo).Include(su => su.Setor).ToListAsync();
            var setoresList = setoresUsuario.Select(su => new { id = su.SetorId, nome = su.Setor?.Nome, organizacaoId = su.Setor?.OrganizacaoId, organizacaoUnidadeId = su.Setor?.OrganizacaoUnidadeId }).ToList();
            // derive unique unidades and organizacoes
            var unidadesIds = setoresList.Where(s => s.organizacaoUnidadeId.HasValue).Select(s => s.organizacaoUnidadeId!.Value).Distinct().ToList();
            var orgIds = setoresList.Where(s => s.organizacaoId.HasValue).Select(s => s.organizacaoId!.Value).Distinct().ToList();
            var unidades = await _db.OrganizacaoUnidades.Where(u2 => unidadesIds.Contains(u2.Id)).Select(u2 => new { id = u2.Id, nome = u2.Nome, organizacaoId = u2.OrganizacaoId }).ToListAsync();
            var orgs = await _db.Organizacoes.Where(o => orgIds.Contains(o.Id)).Select(o => new { id = o.Id, nome = o.Nome }).ToListAsync();

            return OkData(new { administrado = false, organizacoes = orgs, unidades, setores = setoresList, ultimoAcesso = new { organizacaoId = u.UltimoAcessoOrganizacaoId, organizacaoUnidadeId = u.UltimoAcessoOrganizacaoUnidadeId, setorId = u.UltimoAcessoSetorId } });
        }
    }
}
