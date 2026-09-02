using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class OrganizacaoServico : ServicoBase<Organizacao, OrganizacaoDto>, IOrganizacaoServico
    {
        private readonly IOrganizacaoRepositorio _repositorioConcrete;

        public OrganizacaoServico(IOrganizacaoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override OrganizacaoDto ToDto(Organizacao e)
        {
            return new OrganizacaoDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Codigo = e.Codigo,
                Sigla = e.Sigla,
                PessoaId = e.PessoaId,
                TipoId = e.TipoId,
                SituacaoId = e.SituacaoId,
                OrganizacaoPaiId = e.OrganizacaoPaiId,
                OrganizacaoRaizId = e.OrganizacaoRaizId,
                Nivel = e.Nivel,
                Ativo = e.Ativo
            };
        }

        protected override Organizacao FromDto(OrganizacaoDto dto)
        {
            return new Organizacao
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Sigla = dto.Sigla,
                PessoaId = dto.PessoaId,
                TipoId = dto.TipoId,
                SituacaoId = dto.SituacaoId,
                OrganizacaoPaiId = dto.OrganizacaoPaiId,
                OrganizacaoRaizId = dto.OrganizacaoRaizId,
                Nivel = dto.Nivel,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Organizacao entity, OrganizacaoDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Sigla = dto.Sigla;
            entity.PessoaId = dto.PessoaId;
            entity.TipoId = dto.TipoId;
            entity.SituacaoId = dto.SituacaoId;
            entity.OrganizacaoPaiId = dto.OrganizacaoPaiId;
            entity.OrganizacaoRaizId = dto.OrganizacaoRaizId;
            entity.Nivel = dto.Nivel;
            entity.Ativo = dto.Ativo;
        }
    }
}
