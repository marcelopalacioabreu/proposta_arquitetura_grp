using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class TipoUnidadeServico : ServicoBase<TipoUnidade, TipoUnidadeDto>, ITipoUnidadeServico
    {
        private readonly ITipoUnidadeRepositorio _repositorioConcrete;

        public TipoUnidadeServico(ITipoUnidadeRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override TipoUnidadeDto ToDto(TipoUnidade e)
        {
            return new TipoUnidadeDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override TipoUnidade FromDto(TipoUnidadeDto dto)
        {
            return new TipoUnidade
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(TipoUnidade entity, TipoUnidadeDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
