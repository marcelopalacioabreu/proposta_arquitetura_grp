using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class OrganizacaoUnidadeSetorServico : ServicoBase<OrganizacaoUnidadeSetor, OrganizacaoUnidadeSetorDto>, IOrganizacaoUnidadeSetorServico
    {
        private readonly IOrganizacaoUnidadeSetorRepositorio _repositorioConcrete;

        public OrganizacaoUnidadeSetorServico(IOrganizacaoUnidadeSetorRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override OrganizacaoUnidadeSetorDto ToDto(OrganizacaoUnidadeSetor e)
        {
            return new OrganizacaoUnidadeSetorDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override OrganizacaoUnidadeSetor FromDto(OrganizacaoUnidadeSetorDto dto)
        {
            return new OrganizacaoUnidadeSetor
            {
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(OrganizacaoUnidadeSetor entity, OrganizacaoUnidadeSetorDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
