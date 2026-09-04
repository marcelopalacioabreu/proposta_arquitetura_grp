using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.Servicos.Utils;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Exceptions;

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

        /// <summary>
        /// Valida os dados de uma unidade organizacional
        /// </summary>
        private void ValidarUnidade(OrganizacaoUnidadeDto dto)
        {
            var erros = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(dto.Nome))
                erros["nome"] = new[] { "Nome é obrigatório" };

            if (!dto.TipoId.HasValue || dto.TipoId <= 0)
                erros["tipoId"] = new[] { "Tipo de unidade é obrigatório" };

            if (!dto.SituacaoId.HasValue || dto.SituacaoId <= 0)
                erros["situacaoId"] = new[] { "Situação é obrigatória" };

            // Validar Pessoa Jurídica se fornecida
            if (!string.IsNullOrWhiteSpace(dto.PessoaRazaoSocial))
            {
                if (string.IsNullOrWhiteSpace(dto.PessoaRazaoSocial))
                    erros["pessoaRazaoSocial"] = new[] { "Razão Social é obrigatória" };

                // Validar CNPJ se fornecido
                if (!string.IsNullOrWhiteSpace(dto.PessoaCnpj))
                {
                    if (!CnpjValidator.Validar(dto.PessoaCnpj))
                        erros["pessoaCnpj"] = new[] { "CNPJ inválido. Aceitos formatos numéricos (14 dígitos) ou alphanumeric" };
                }

                // Validar datas de Pessoa Jurídica
                if (dto.PessoaDataFundacao.HasValue && dto.PessoaDataExtincao.HasValue)
                {
                    if (dto.PessoaDataExtincao < dto.PessoaDataFundacao)
                        erros["pessoaDataExtincao"] = new[] { "Data de extinção não pode ser anterior à data de fundação" };
                }
            }

            if (erros.Count > 0)
                throw new ValidationException("Validação de unidade falhou", erros);
        }

        public override async Task<OrganizacaoUnidadeDto> CriarAsync(OrganizacaoUnidadeDto dto)
        {
            ValidarUnidade(dto);
            return await base.CriarAsync(dto);
        }

        public override async Task UpdateAsync(long id, OrganizacaoUnidadeDto dto)
        {
            ValidarUnidade(dto);
            await base.UpdateAsync(id, dto);
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
                OrganizacaoCodigo = e.Organizacao?.Codigo,
                UnidadePaiId = e.UnidadePaiId,
                TipoId = e.TipoId,
                SituacaoId = e.SituacaoId,
                ResponsavelPessoaId = e.ResponsavelId,
                PessoaId = e.PessoaId,
                Nivel = e.Nivel,
                DataFundacao = e.DataFundacao,
                DataExtincao = e.DataExtincao,
                DataInsercao = e.DataInsercao,
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
