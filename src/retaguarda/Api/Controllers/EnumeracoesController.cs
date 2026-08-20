using Microsoft.AspNetCore.Mvc;
using Retaguarda.Dominio.Entidades.Enumeracoes;

namespace Retaguarda.Api.Controllers
{
    /// <summary>
    /// Controller para servir enumerações do sistema via API
    /// </summary>
    [ApiController]
    [Route("api/enumeracoes")]
    public class EnumeracoesController : BaseController
    {
        /// <summary>
        /// Obtém uma enumeração pelo nome
        /// GET /api/enumeracoes/pessoa.tipos
        /// </summary>
        [HttpGet("{nome}")]
        public IActionResult GetEnumeracao(string nome)
        {
            var items = nome switch
            {
                "pessoa.tipos" => ObterPessoaTipos(),
                "pessoa.situacao" => ObterPessoaSituacao(),
                _ => null
            };

            if (items == null)
                return NotFoundError($"Enumeração '{nome}' não encontrada");

            // Retornar no formato esperado: { items: [...] }
            return OkData(new { items = items });
        }

        /// <summary>
        /// Obtém todos os tipos de pessoa (Física, Jurídica)
        /// </summary>
        private object[] ObterPessoaTipos()
        {
            return new object[]
            {
                new
                {
                    id = PessoaTipo.FISICA,
                    chave = PessoaTipo.FISICA,
                    descricao = "Física",
                    nome = "Física"
                },
                new
                {
                    id = PessoaTipo.JURIDICA,
                    chave = PessoaTipo.JURIDICA,
                    descricao = "Jurídica",
                    nome = "Jurídica"
                }
            };
        }

        /// <summary>
        /// Obtém todos os status de pessoa (Ativa, Inativa)
        /// </summary>
        private object[] ObterPessoaSituacao()
        {
            return new object[]
            {
                new
                {
                    id = "A",
                    chave = "A",
                    descricao = "Ativa",
                    nome = "Ativa"
                },
                new
                {
                    id = "I",
                    chave = "I",
                    descricao = "Inativa",
                    nome = "Inativa"
                }
            };
        }
    }
}

