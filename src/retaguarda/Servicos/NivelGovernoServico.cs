using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class NivelGovernoServico : ServicoBase<NivelGoverno, NivelGovernoDto>, INivelGovernoServico
    {
        private readonly INivelGovernoRepositorio _repositorioConcrete;

        public NivelGovernoServico(INivelGovernoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override NivelGovernoDto ToDto(NivelGoverno e)
        {
            return new NivelGovernoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override NivelGoverno FromDto(NivelGovernoDto dto)
        {
            return new NivelGoverno
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(NivelGoverno entity, NivelGovernoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
