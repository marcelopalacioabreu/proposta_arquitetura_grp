using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class TipoImovelServico : ServicoBase<TipoImovel, TipoImovelDto>, ITipoImovelServico
    {
        private readonly ITipoImovelRepositorio _repositorioConcrete;

        public TipoImovelServico(ITipoImovelRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override TipoImovelDto ToDto(TipoImovel e)
        {
            return new TipoImovelDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override TipoImovel FromDto(TipoImovelDto dto)
        {
            return new TipoImovel
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(TipoImovel entity, TipoImovelDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
