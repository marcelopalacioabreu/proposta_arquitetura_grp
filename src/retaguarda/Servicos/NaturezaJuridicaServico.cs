using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class NaturezaJuridicaServico : ServicoBase<NaturezaJuridica, NaturezaJuridicaDto>, INaturezaJuridicaServico
    {
        private readonly INaturezaJuridicaRepositorio _repositorioConcrete;

        public NaturezaJuridicaServico(INaturezaJuridicaRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override NaturezaJuridicaDto ToDto(NaturezaJuridica e)
        {
            return new NaturezaJuridicaDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override NaturezaJuridica FromDto(NaturezaJuridicaDto dto)
        {
            return new NaturezaJuridica
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(NaturezaJuridica entity, NaturezaJuridicaDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
