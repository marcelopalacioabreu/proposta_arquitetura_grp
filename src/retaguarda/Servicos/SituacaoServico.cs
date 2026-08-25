using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class SituacaoServico : ServicoBase<Situacao, SituacaoDto>, ISituacaoServico
    {
        private readonly ISituacaoRepositorio _repositorioConcrete;

        public SituacaoServico(ISituacaoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override SituacaoDto ToDto(Situacao e)
        {
            return new SituacaoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Contexto = e.Contexto,
                Descricao = e.Descricao,
                Ativo = e.Ativo
            };
        }

        protected override Situacao FromDto(SituacaoDto dto)
        {
            return new Situacao
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Contexto = dto.Contexto,
                Descricao = dto.Descricao,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Situacao entity, SituacaoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Contexto = dto.Contexto;
            entity.Descricao = dto.Descricao;
            entity.Ativo = dto.Ativo;
        }
    }
}
