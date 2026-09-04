using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Dominio.Entidades.Enumeracoes;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class PessoaServico : ServicoBase<Pessoa, PessoaDto>, IPessoaServico
    {
        private readonly IPessoaRepositorio _repositorioConcrete;

        public PessoaServico(IPessoaRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override PessoaDto ToDto(Pessoa e)
        {
            var dto = new PessoaDto { Id = e.Id, TipoPessoa = e.TipoPessoa.Chave, DataInsercao = e.DataInsercao, Ativo = e.Ativo };
            if (e is PessoaFisica pf)
            {
                dto.Nome = pf.Nome; dto.NomeSocial = pf.NomeSocial; dto.Cpf = pf.Cpf;
                dto.DataNascimento = pf.DataNascimento; dto.Sexo = pf.Sexo?.Chave;
                dto.EstadoCivil = pf.EstadoCivil?.Chave; dto.NomeMae = pf.NomeMae;
                dto.NomePai = pf.NomePai; dto.Pcd = pf.Pcd; dto.DataObito = pf.DataObito;
            }
            else if (e is PessoaJuridica pj)
            {
                dto.RazaoSocial = pj.RazaoSocial; dto.NomeFantasia = pj.NomeFantasia;
                dto.Cnpj = pj.Cnpj; dto.DataFundacao = pj.DataFundacao;
                dto.DataExtincao = pj.DataExtincao; dto.InscricaoEstadual = pj.InscricaoEstadual;
                dto.InscricaoMunicipal = pj.InscricaoMunicipal; dto.SituacaoId = pj.SituacaoId;
            }
            return dto;
        }

        protected override Pessoa FromDto(PessoaDto dto)
        {
            if (dto.TipoPessoa == PessoaTipo.JURIDICA)
                return new PessoaJuridica
                {
                    TipoPessoa = PessoaTipo.Juridica, Ativo = dto.Ativo,
                    RazaoSocial = dto.RazaoSocial ?? string.Empty,
                    NomeFantasia = dto.NomeFantasia ?? string.Empty,
                    Cnpj = dto.Cnpj ?? string.Empty, DataFundacao = dto.DataFundacao,
                    DataExtincao = dto.DataExtincao,
                    InscricaoEstadual = dto.InscricaoEstadual ?? string.Empty,
                    InscricaoMunicipal = dto.InscricaoMunicipal ?? string.Empty,
                    SituacaoId = dto.SituacaoId
                };

            return new PessoaFisica
            {
                TipoPessoa = PessoaTipo.Fisica, Ativo = dto.Ativo,
                Nome = dto.Nome ?? string.Empty,
                NomeSocial = dto.NomeSocial ?? string.Empty,
                Cpf = dto.Cpf ?? string.Empty, DataNascimento = dto.DataNascimento,
                Sexo = dto.Sexo != null ? Sexo.ObterPorChave(dto.Sexo) : Sexo.Masculino,
                EstadoCivil = dto.EstadoCivil != null ? EstadoCivil.ObterPorChave(dto.EstadoCivil) : EstadoCivil.Solteiro,
                NomeMae = dto.NomeMae ?? string.Empty, NomePai = dto.NomePai ?? string.Empty,
                Pcd = dto.Pcd, DataObito = dto.DataObito
            };
        }

        protected override void UpdateEntityFromDto(Pessoa entity, PessoaDto dto)
        {
            entity.Ativo = dto.Ativo;
            if (entity is PessoaFisica pf)
            {
                if (dto.Nome != null) pf.Nome = dto.Nome;
                if (dto.NomeSocial != null) pf.NomeSocial = dto.NomeSocial;
                if (dto.Cpf != null) pf.Cpf = dto.Cpf;
                if (dto.DataNascimento.HasValue) pf.DataNascimento = dto.DataNascimento;
                if (dto.Sexo != null) pf.Sexo = Sexo.ObterPorChave(dto.Sexo);
                if (dto.EstadoCivil != null) pf.EstadoCivil = EstadoCivil.ObterPorChave(dto.EstadoCivil);
                if (dto.NomeMae != null) pf.NomeMae = dto.NomeMae;
                if (dto.NomePai != null) pf.NomePai = dto.NomePai;
                pf.Pcd = dto.Pcd;
                if (dto.DataObito.HasValue) pf.DataObito = dto.DataObito;
            }
            else if (entity is PessoaJuridica pj)
            {
                if (dto.RazaoSocial != null) pj.RazaoSocial = dto.RazaoSocial;
                if (dto.NomeFantasia != null) pj.NomeFantasia = dto.NomeFantasia;
                if (dto.Cnpj != null) pj.Cnpj = dto.Cnpj;
                if (dto.DataFundacao.HasValue) pj.DataFundacao = dto.DataFundacao;
                if (dto.DataExtincao.HasValue) pj.DataExtincao = dto.DataExtincao;
                if (dto.InscricaoEstadual != null) pj.InscricaoEstadual = dto.InscricaoEstadual;
                if (dto.InscricaoMunicipal != null) pj.InscricaoMunicipal = dto.InscricaoMunicipal;
                if (dto.SituacaoId.HasValue) pj.SituacaoId = dto.SituacaoId;
            }
        }
    }
}

