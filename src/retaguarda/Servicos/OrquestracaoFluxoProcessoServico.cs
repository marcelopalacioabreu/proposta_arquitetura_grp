using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Servicos.Base;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class OrquestracaoFluxoProcessoServico : ServicoBase<OrquestracaoFluxoProcesso, OrquestracaoFluxoProcessoDto>, IOrquestracaoFluxoProcessoServico
    {
        public OrquestracaoFluxoProcessoServico(IOrquestracaoFluxoProcessoRepositorio repositorio) : base(repositorio)
        {
        }

        protected override OrquestracaoFluxoProcessoDto ToDto(OrquestracaoFluxoProcesso e)
        {
            return new OrquestracaoFluxoProcessoDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Descricao = e.Descricao,
                WorkflowDefinitionId = e.WorkflowDefinitionId,
                WorkflowVersion = e.WorkflowVersion,
                Ativo = e.Ativo,
                WorkflowJson = e.WorkflowJson,
                WorkflowNome = e.WorkflowNome
            };
        }

        protected override OrquestracaoFluxoProcesso FromDto(OrquestracaoFluxoProcessoDto dto)
        {
            return new OrquestracaoFluxoProcesso
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                WorkflowDefinitionId = dto.WorkflowDefinitionId,
                WorkflowVersion = dto.WorkflowVersion,
                Ativo = dto.Ativo,
                WorkflowJson = dto.WorkflowJson,
                WorkflowNome = dto.WorkflowNome
            };
        }

        protected override void UpdateEntityFromDto(OrquestracaoFluxoProcesso entity, OrquestracaoFluxoProcessoDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Descricao = dto.Descricao;
            entity.WorkflowDefinitionId = dto.WorkflowDefinitionId;
            entity.WorkflowVersion = dto.WorkflowVersion;
            entity.Ativo = dto.Ativo;
            entity.WorkflowJson = dto.WorkflowJson;
            entity.WorkflowNome = dto.WorkflowNome;
        }
    }
}
