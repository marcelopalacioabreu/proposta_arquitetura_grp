using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Exceptions;

namespace Retaguarda.Servicos
{
    public class OrganizacaoServico : ServicoBase<Organizacao, OrganizacaoDto>, IOrganizacaoServico
    {
        private readonly IOrganizacaoRepositorio _repositorioConcrete;

        public OrganizacaoServico(IOrganizacaoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        /// <summary>
        /// Valida os dados obrigatórios de uma organização.
        /// Lança ValidationException se houver erros.
        /// </summary>
        private void ValidarOrganizacao(OrganizacaoDto dto)
        {
            var erros = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(dto.Sigla))
                erros["sigla"] = new[] { "Sigla é obrigatória" };

            if (string.IsNullOrWhiteSpace(dto.Nome))
                erros["nome"] = new[] { "Nome é obrigatório" };

            if (!dto.TipoId.HasValue || dto.TipoId <= 0)
                erros["tipoId"] = new[] { "Tipo de organização é obrigatório" };

            if (!dto.SituacaoId.HasValue || dto.SituacaoId <= 0)
                erros["situacaoId"] = new[] { "Situação é obrigatória" };

            if (erros.Count > 0)
                throw new ValidationException("Validação de organização falhou", erros);
        }

        /// <summary>
        /// Sobrescreve CriarAsync para adicionar validações de negócio
        /// </summary>
        public override async Task<OrganizacaoDto> CriarAsync(OrganizacaoDto dto)
        {
            ValidarOrganizacao(dto);
            return await base.CriarAsync(dto);
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
