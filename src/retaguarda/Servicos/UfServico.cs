using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class UfServico : ServicoBase<Uf, UfDto>, IUfServico
    {
        private readonly IUfRepositorio _repositorioConcrete;

        public UfServico(IUfRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override UfDto ToDto(Uf e)
        {
            return new UfDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Sigla = e.Sigla,
                PaisId = e.PaisId,
                Ativo = e.Ativo
            };
        }

        protected override Uf FromDto(UfDto dto)
        {
            return new Uf
            {
                Nome = dto.Nome,
                Sigla = dto.Sigla,
                PaisId = dto.PaisId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Uf entity, UfDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Sigla = dto.Sigla;
            entity.PaisId = dto.PaisId;
            entity.Ativo = dto.Ativo;
        }
    }
}
