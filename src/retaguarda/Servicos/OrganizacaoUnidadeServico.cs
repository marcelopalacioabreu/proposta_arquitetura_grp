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
                TipoUnidadeId = e.TipoUnidadeId,
                UnidadePaiId = e.UnidadePaiId,
                Cnpj = e.Cnpj,
                SituacaoId = e.SituacaoId,
                ResponsavelPessoaId = e.ResponsavelPessoaId,
                DataFundacao = e.DataFundacao,
                DataExtincao = e.DataExtincao,
                HierarquiaCodigo = e.HierarquiaCodigo,
                HierarquiaNome = e.HierarquiaNome,
                Nivel = e.Nivel,
                ValidoDe = e.ValidoDe,
                ValidoAte = e.ValidoAte,
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
                TipoUnidadeId = dto.TipoUnidadeId,
                UnidadePaiId = dto.UnidadePaiId,
                Cnpj = dto.Cnpj,
                SituacaoId = dto.SituacaoId,
                ResponsavelPessoaId = dto.ResponsavelPessoaId,
                DataFundacao = dto.DataFundacao,
                DataExtincao = dto.DataExtincao,
                HierarquiaCodigo = dto.HierarquiaCodigo,
                HierarquiaNome = dto.HierarquiaNome,
                Nivel = dto.Nivel,
                ValidoDe = dto.ValidoDe,
                ValidoAte = dto.ValidoAte,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(OrganizacaoUnidade entity, OrganizacaoUnidadeDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Sigla = dto.Sigla;
            entity.TipoUnidadeId = dto.TipoUnidadeId;
            entity.UnidadePaiId = dto.UnidadePaiId;
            entity.Cnpj = dto.Cnpj;
            entity.SituacaoId = dto.SituacaoId;
            entity.ResponsavelPessoaId = dto.ResponsavelPessoaId;
            entity.DataFundacao = dto.DataFundacao;
            entity.DataExtincao = dto.DataExtincao;
            entity.HierarquiaCodigo = dto.HierarquiaCodigo;
            entity.HierarquiaNome = dto.HierarquiaNome;
            entity.Nivel = dto.Nivel;
            entity.ValidoDe = dto.ValidoDe;
            entity.ValidoAte = dto.ValidoAte;
            entity.Ativo = dto.Ativo;
        }
    }
}
