using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class PaisServico : ServicoBase<EnderecoPais, EnderecoPaisDto>, IPaisServico
    {
        private readonly IPaisRepositorio _repositorioConcrete;

        public PaisServico(IPaisRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override EnderecoPaisDto ToDto(EnderecoPais e)
        {
            return new EnderecoPaisDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Codigo = e.Codigo,
                Ativo = e.Ativo
            };
        }

        protected override EnderecoPais FromDto(EnderecoPaisDto dto)
        {
            return new EnderecoPais
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoPais entity, EnderecoPaisDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Ativo = dto.Ativo;
        }
    }
}
