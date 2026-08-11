using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class PaisServico : ServicoBase<Pais, PaisDto>, IPaisServico
    {
        private readonly IPaisRepositorio _repositorioConcrete;

        public PaisServico(IPaisRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override PaisDto ToDto(Pais e)
        {
            return new PaisDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Codigo = e.Codigo,
                Ativo = e.Ativo
            };
        }

        protected override Pais FromDto(PaisDto dto)
        {
            return new Pais
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Pais entity, PaisDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Ativo = dto.Ativo;
        }
    }
}
