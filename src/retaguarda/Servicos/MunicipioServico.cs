using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class MunicipioServico : ServicoBase<EnderecoMunicipio, EnderecoMunicipioDto>, IMunicipioServico
    {
        private readonly IMunicipioRepositorio _repositorioConcrete;

        public MunicipioServico(IMunicipioRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override EnderecoMunicipioDto ToDto(EnderecoMunicipio e)
        {
            return new EnderecoMunicipioDto
            {
                Id = e.Id,
                Nome = e.Nome,
                CodigoIbge = e.CodigoIbge,
                UfId = e.UfId,
                Ativo = e.Ativo
            };
        }

        protected override EnderecoMunicipio FromDto(EnderecoMunicipioDto dto)
        {
            return new EnderecoMunicipio
            {
                Id = dto.Id,
                Nome = dto.Nome,
                CodigoIbge = dto.CodigoIbge,
                UfId = dto.UfId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoMunicipio entity, EnderecoMunicipioDto dto)
        {
            entity.Nome = dto.Nome;
            entity.CodigoIbge = dto.CodigoIbge;
            entity.UfId = dto.UfId;
            entity.Ativo = dto.Ativo;
        }
    }
}
