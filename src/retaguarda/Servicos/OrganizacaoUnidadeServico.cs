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
        private readonly IRepositorioBase<Pessoa> _pessoaRepositorio;

        public OrganizacaoUnidadeServico(
            IOrganizacaoUnidadeRepositorio repositorio,
            IRepositorioBase<Pessoa> pessoaRepositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
            _pessoaRepositorio = pessoaRepositorio;
        }

        protected override OrganizacaoUnidadeDto ToDto(OrganizacaoUnidade e)
        {
            var dto = new OrganizacaoUnidadeDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Codigo = e.Codigo,
                Sigla = e.Sigla,
                Descricao = e.Descricao,
                OrganizacaoId = e.OrganizacaoId,
                UnidadePaiId = e.UnidadePaiId,
                TipoId = e.TipoId,
                SituacaoId = e.SituacaoId,
                ResponsavelPessoaId = e.ResponsavelId,
                PessoaId = e.PessoaId,
                Nivel = e.Nivel,
                DataFundacao = e.DataFundacao,
                DataExtincao = e.DataExtincao,
                Ativo = e.Ativo
            };

            // Map Pessoa Jurídica fields if exists
            if (e.Pessoa is PessoaJuridica pessoaJuridica)
            {
                dto.PessoaRazaoSocial = pessoaJuridica.RazaoSocial;
                dto.PessoaNomeFantasia = pessoaJuridica.NomeFantasia;
                dto.PessoaDataFundacao = pessoaJuridica.DataFundacao;
                dto.PessoaDataExtincao = pessoaJuridica.DataExtincao;
                dto.PessoaCnpj = pessoaJuridica.Cnpj;
                dto.PessoaAnotacoes = pessoaJuridica.Anotacoes;
                dto.PessoaInscricaoEstadual = pessoaJuridica.InscricaoEstadual;
                dto.PessoaInscricaoMunicipal = pessoaJuridica.InscricaoMunicipal;
            }

            return dto;
        }

        protected override OrganizacaoUnidade FromDto(OrganizacaoUnidadeDto dto)
        {
            var unidade = new OrganizacaoUnidade
            {
                Nome = dto.Nome,
                Codigo = dto.Codigo,
                Sigla = dto.Sigla,
                Descricao = dto.Descricao,
                OrganizacaoId = dto.OrganizacaoId,
                UnidadePaiId = dto.UnidadePaiId,
                TipoId = dto.TipoId,
                SituacaoId = dto.SituacaoId,
                ResponsavelId = dto.ResponsavelPessoaId,
                Nivel = dto.Nivel,
                DataFundacao = dto.DataFundacao,
                DataExtincao = dto.DataExtincao,
                Ativo = dto.Ativo
            };

            // Create or update Pessoa Jurídica if data is provided
            if (!string.IsNullOrWhiteSpace(dto.PessoaRazaoSocial))
            {
                var pessoaJuridica = new PessoaJuridica
                {
                    RazaoSocial = dto.PessoaRazaoSocial,
                    NomeFantasia = dto.PessoaNomeFantasia ?? string.Empty,
                    DataFundacao = dto.PessoaDataFundacao,
                    DataExtincao = dto.PessoaDataExtincao,
                    Cnpj = dto.PessoaCnpj ?? string.Empty,
                    Anotacoes = dto.PessoaAnotacoes ?? string.Empty,
                    InscricaoEstadual = dto.PessoaInscricaoEstadual ?? string.Empty,
                    InscricaoMunicipal = dto.PessoaInscricaoMunicipal ?? string.Empty,
                    Ativo = true
                };

                unidade.Pessoa = pessoaJuridica;
            }
            else if (dto.PessoaId.HasValue)
            {
                // If PessoaId is provided but no PessoaRazaoSocial, just reference existing person
                unidade.PessoaId = dto.PessoaId.Value;
            }

            return unidade;
        }

        protected override void UpdateEntityFromDto(OrganizacaoUnidade entity, OrganizacaoUnidadeDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Codigo = dto.Codigo;
            entity.Sigla = dto.Sigla;
            entity.Descricao = dto.Descricao;
            entity.OrganizacaoId = dto.OrganizacaoId;
            entity.UnidadePaiId = dto.UnidadePaiId;
            entity.TipoId = dto.TipoId;
            entity.SituacaoId = dto.SituacaoId;
            entity.ResponsavelId = dto.ResponsavelPessoaId;
            entity.Nivel = dto.Nivel;
            entity.DataFundacao = dto.DataFundacao;
            entity.DataExtincao = dto.DataExtincao;
            entity.Ativo = dto.Ativo;

            // Update or create Pessoa Jurídica if data is provided
            if (!string.IsNullOrWhiteSpace(dto.PessoaRazaoSocial))
            {
                if (entity.Pessoa is PessoaJuridica pessoaJuridica)
                {
                    // Update existing Pessoa Jurídica
                    pessoaJuridica.RazaoSocial = dto.PessoaRazaoSocial;
                    pessoaJuridica.NomeFantasia = dto.PessoaNomeFantasia ?? string.Empty;
                    pessoaJuridica.DataFundacao = dto.PessoaDataFundacao;
                    pessoaJuridica.DataExtincao = dto.PessoaDataExtincao;
                    pessoaJuridica.Cnpj = dto.PessoaCnpj ?? string.Empty;
                    pessoaJuridica.Anotacoes = dto.PessoaAnotacoes ?? string.Empty;
                    pessoaJuridica.InscricaoEstadual = dto.PessoaInscricaoEstadual ?? string.Empty;
                    pessoaJuridica.InscricaoMunicipal = dto.PessoaInscricaoMunicipal ?? string.Empty;
                }
                else
                {
                    // Create new Pessoa Jurídica if it doesn't exist
                    var novaPessoa = new PessoaJuridica
                    {
                        RazaoSocial = dto.PessoaRazaoSocial,
                        NomeFantasia = dto.PessoaNomeFantasia ?? string.Empty,
                        DataFundacao = dto.PessoaDataFundacao,
                        DataExtincao = dto.PessoaDataExtincao,
                        Cnpj = dto.PessoaCnpj ?? string.Empty,
                        Anotacoes = dto.PessoaAnotacoes ?? string.Empty,
                        InscricaoEstadual = dto.PessoaInscricaoEstadual ?? string.Empty,
                        InscricaoMunicipal = dto.PessoaInscricaoMunicipal ?? string.Empty,
                        Ativo = true
                    };
                    entity.Pessoa = novaPessoa;
                }
            }
        }
    }
}
