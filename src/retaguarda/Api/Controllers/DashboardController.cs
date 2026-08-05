using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Retaguarda.Persistencia.MYSQL;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            // counts
            var organizacoesCount = await _db.Organizacoes.CountAsync();
            var usuariosCount = await _db.Usuarios.CountAsync();
            var perfisCount = await _db.Perfis.CountAsync();

            // aggregate recent activities across main entities using DataAlteracao fallback to DataInsercao
            var orgs = _db.Organizacoes.Select(x => new { Tipo = "Organizacao", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao });
            var users = _db.Usuarios.Select(x => new { Tipo = "Usuario", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao });
            var profiles = _db.Perfis.Select(x => new { Tipo = "Perfil", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao });
            var unidades = _db.OrganizacaoUnidades.Select(x => new { Tipo = "Unidade", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao });
            var setores = _db.OrganizacaoSetores.Select(x => new { Tipo = "Setor", Texto = x.Nome, Data = x.DataAlteracao ?? x.DataInsercao });

            // run the queries and merge in memory to allow ordering across sets
            var list = await Task.WhenAll(
                orgs.Take(10).ToListAsync(),
                users.Take(10).ToListAsync(),
                profiles.Take(10).ToListAsync(),
                unidades.Take(10).ToListAsync(),
                setores.Take(10).ToListAsync()
            );

            var merged = list.SelectMany(x => x).Where(x => x.Data != null).OrderByDescending(x => x.Data).Take(5).Select(x => new { tipo = x.Tipo, texto = x.Texto, data = x.Data }).ToList();

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
    }
}
