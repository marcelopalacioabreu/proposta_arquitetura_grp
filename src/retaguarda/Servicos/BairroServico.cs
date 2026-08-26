using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class BairroServico : ServicoBase<EnderecoBairro, EnderecoBairroDto>, IBairroServico
    {
        private readonly IBairroRepositorio _repositorioConcrete;

        public BairroServico(IBairroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override EnderecoBairroDto ToDto(EnderecoBairro e)
        {
            return new EnderecoBairroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                MunicipioId = e.MunicipioId,
                Ativo = e.Ativo
            };
        }

        protected override EnderecoBairro FromDto(EnderecoBairroDto dto)
        {
            return new EnderecoBairro
            {
                Id = dto.Id,
                Nome = dto.Nome,
                MunicipioId = dto.MunicipioId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoBairro entity, EnderecoBairroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.MunicipioId = dto.MunicipioId;
            entity.Ativo = dto.Ativo;
        }
    }
}
