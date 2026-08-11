using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class BairroServico : ServicoBase<Bairro, BairroDto>, IBairroServico
    {
        private readonly IBairroRepositorio _repositorioConcrete;

        public BairroServico(IBairroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override BairroDto ToDto(Bairro e)
        {
            return new BairroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                MunicipioId = e.MunicipioId,
                Ativo = e.Ativo
            };
        }

        protected override Bairro FromDto(BairroDto dto)
        {
            return new Bairro
            {
                Nome = dto.Nome,
                MunicipioId = dto.MunicipioId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Bairro entity, BairroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.MunicipioId = dto.MunicipioId;
            entity.Ativo = dto.Ativo;
        }
    }
}
