using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly Retaguarda.Persistencia.IApplicationDbContext _db;

        public DashboardController(Retaguarda.Persistencia.IApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                // counts
                var organizacoesCount = await _db.Organizacoes.CountAsync();
                var usuariosCount = await _db.Usuarios.CountAsync();
                var perfisCount = await _db.Perfis.CountAsync();

                // aggregate recent activities across main entities using DataAlteracao fallback to DataInsercao
                var orgs = await _db.Organizacoes.Select(x => new { Tipo = "Organizacao", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao }).Take(10).ToListAsync();
                var users = await _db.Usuarios.Select(x => new { Tipo = "Usuario", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao }).Take(10).ToListAsync();
                var profiles = await _db.Perfis.Select(x => new { Tipo = "Perfil", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao }).Take(10).ToListAsync();
                var unidades = await _db.OrganizacaoUnidades.Select(x => new { Tipo = "Unidade", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao }).Take(10).ToListAsync();
                var setores = await _db.OrganizacaoSetores.Select(x => new { Tipo = "Setor", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao }).Take(10).ToListAsync();

                var merged = orgs.Concat(users).Concat(profiles).Concat(unidades).Concat(setores).Where(x => x.Data != null).OrderByDescending(x => x.Data).Take(5).Select(x => new { tipo = x.Tipo, texto = x.Texto, data = x.Data }).ToList();

                // shortcuts (simple repaginada)
                var atalhos = new[] {
                    new { label = "Organizações", path = "/painel/organizacoes", variant = "primary" },
                    new { label = "Usuários", path = "/painel/usuarios", variant = "secondary" },
                    new { label = "Perfis", path = "/painel/perfis", variant = "secondary" }
                };

                var data = new {
                    contadores = new { organizacoes = organizacoesCount, usuarios = usuariosCount, perfis = perfisCount },
                    atividades = merged,
                    atalhos
                };

                return OkData(data);
            }
            catch (System.Exception ex)
            {
                try { System.Console.WriteLine($"DashboardController.Get error: {ex}"); } catch { }
                return Error("Erro ao carregar dashboard", 500, new { message = ex.Message });
            }
        }
    }
}
