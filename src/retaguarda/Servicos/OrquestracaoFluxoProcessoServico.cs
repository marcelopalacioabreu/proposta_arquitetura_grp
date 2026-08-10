using System.Threading.Tasks;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Dominio.Entidades;
using System.Collections.Generic;

namespace Retaguarda.Servicos
{
    public class OrquestracaoFluxoProcessoServico : IOrquestracaoFluxoProcessoServico
    {
        private readonly IOrquestracaoFluxoProcessoRepositorio _repositorio;

        public OrquestracaoFluxoProcessoServico(IOrquestracaoFluxoProcessoRepositorio repositorio) => _repositorio = repositorio;

        public async Task<OrquestracaoFluxoProcesso?> ObterPorIdAsync(long id) => await _repositorio.ObterPorIdAsync(id);

        public async Task<(List<OrquestracaoFluxoProcesso> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, string? campo = null, string? operador = null, string? valor = null, string? valorDe = null, string? valorAte = null, int? inativo = null)
            => await _repositorio.ListarAsync(nomeFilter, page, pageSize, sortField, sortDir, campo, operador, valor, valorDe, valorAte, inativo);

        public async Task<OrquestracaoFluxoProcesso> CriarAsync(string nome, string? descricao = null, string? workflowDefinitionId = null, int? workflowVersion = null)
        {
            var o = new OrquestracaoFluxoProcesso { Nome = nome, Descricao = descricao ?? string.Empty, WorkflowDefinitionId = workflowDefinitionId, WorkflowVersion = workflowVersion };
            return await _repositorio.AdicionarAsync(o);
        }

        public async Task DeleteAsync(long id) => await _repositorio.DeleteAsync(id);
        public async Task RestaurarAsync(long id) => await _repositorio.RestaurarAsync(id);
        public async Task UpdateAsync(OrquestracaoFluxoProcesso o) => await _repositorio.UpdateAsync(o);
    }
}
