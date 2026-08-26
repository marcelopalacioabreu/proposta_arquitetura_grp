using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class OrganizacaoUnidadeServico : ServicoBase<OrganizacaoUnidade, OrganizacaoUnidadeDto>, IOrganizacaoUnidadeServico
    {
        private readonly IOrganizacaoUnidadeRepositorio _repositorioConcrete;

        public OrganizacaoUnidadeServico(IOrganizacaoUnidadeRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override OrganizacaoUnidadeDto ToDto(OrganizacaoUnidade e)
        {
            return new OrganizacaoUnidadeDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Codigo = e.Codigo,
                Sigla = e.Sigla,
                UnidadePaiId = e.UnidadePaiId,
                Nivel = e.Nivel,
                Ativo = e.Ativo
            };
        }

        protected override OrganizacaoUnidade FromDto(OrganizacaoUnidadeDto dto)
        {
            return new OrganizacaoUnidade
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Sigla = dto.Sigla,
                UnidadePaiId = dto.UnidadePaiId,
                Nivel = dto.Nivel,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(OrganizacaoUnidade entity, OrganizacaoUnidadeDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Sigla = dto.Sigla;
            entity.UnidadePaiId = dto.UnidadePaiId;
            entity.Nivel = dto.Nivel;
            entity.Ativo = dto.Ativo;
        }
    }
}
