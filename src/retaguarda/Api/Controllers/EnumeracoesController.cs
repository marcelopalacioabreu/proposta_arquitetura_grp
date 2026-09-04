using Microsoft.AspNetCore.Mvc;
using Retaguarda.Dominio.Entidades.Enumeracoes;
using System.Linq;

namespace Retaguarda.Api.Controllers
{
    [ApiController]
    [Route("api/enumeracoes")]
    public class EnumeracoesController : BaseController
    {
        [HttpGet("{tipo}")]
        public IActionResult Get(string tipo)
        {
            var items = tipo.ToLowerInvariant() switch
            {
                "pessoa.tipos" or "pessoa_tipo" =>
                    PessoaTipo.Todos.Select(x => new { id = x.Chave, nome = x.Descricao }).ToList<object>(),
                "sexo" =>
                    Sexo.Todos.Select(x => new { id = x.Chave, nome = x.Descricao }).ToList<object>(),
                "estado_civil" =>
                    EstadoCivil.Todos.Select(x => new { id = x.Chave, nome = x.Descricao }).ToList<object>(),
                _ => null
            };
            if (items == null) return NotFoundError($"Enumeração não encontrada: {tipo}");
            return OkList(items, items.Count, 1, items.Count);
        }
    }
}