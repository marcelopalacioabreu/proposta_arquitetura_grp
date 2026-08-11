using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class MunicipioServico : ServicoBase<Municipio, MunicipioDto>, IMunicipioServico
    {
        private readonly IMunicipioRepositorio _repositorioConcrete;

        public MunicipioServico(IMunicipioRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override MunicipioDto ToDto(Municipio e)
        {
            return new MunicipioDto
            {
                Id = e.Id,
                Nome = e.Nome,
                CodigoIbge = e.CodigoIbge,
                UfId = e.UfId,
                Ativo = e.Ativo
            };
        }

        protected override Municipio FromDto(MunicipioDto dto)
        {
            return new Municipio
            {
                Nome = dto.Nome,
                CodigoIbge = dto.CodigoIbge,
                UfId = dto.UfId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Municipio entity, MunicipioDto dto)
        {
            entity.Nome = dto.Nome;
            entity.CodigoIbge = dto.CodigoIbge;
            entity.UfId = dto.UfId;
            entity.Ativo = dto.Ativo;
        }
    }
}
