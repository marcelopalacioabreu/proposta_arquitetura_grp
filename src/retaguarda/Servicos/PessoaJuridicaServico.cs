using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class PessoaJuridicaServico : ServicoBase<PessoaJuridica, PessoaJuridicaDto>, IPessoaJuridicaServico
    {
        private readonly IRepositorioBase<Pessoa> _repositorioConcrete;

        public PessoaJuridicaServico(IRepositorioBase<Pessoa> repositorio) : base((IRepositorioBase<PessoaJuridica>)(object)repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override PessoaJuridicaDto ToDto(PessoaJuridica e)
        {
            return new PessoaJuridicaDto
            {
                Id = e.Id,
                RazaoSocial = e.RazaoSocial,
                NomeFantasia = e.NomeFantasia,
                DataFundacao = e.DataFundacao,
                DataExtincao = e.DataExtincao,
                Cnpj = e.Cnpj,
                Anotacoes = e.Anotacoes,
                InscricaoEstadual = e.InscricaoEstadual,
                InscricaoMunicipal = e.InscricaoMunicipal,
                Ativo = e.Ativo
            };
        }

        protected override PessoaJuridica FromDto(PessoaJuridicaDto dto)
        {
            return new PessoaJuridica
            {
                RazaoSocial = dto.RazaoSocial,
                NomeFantasia = dto.NomeFantasia,
                DataFundacao = dto.DataFundacao,
                DataExtincao = dto.DataExtincao,
                Cnpj = dto.Cnpj,
                Anotacoes = dto.Anotacoes,
                InscricaoEstadual = dto.InscricaoEstadual,
                InscricaoMunicipal = dto.InscricaoMunicipal,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(PessoaJuridica entity, PessoaJuridicaDto dto)
        {
            entity.RazaoSocial = dto.RazaoSocial;
            entity.NomeFantasia = dto.NomeFantasia;
            entity.DataFundacao = dto.DataFundacao;
            entity.DataExtincao = dto.DataExtincao;
            entity.Cnpj = dto.Cnpj;
            entity.Anotacoes = dto.Anotacoes;
            entity.InscricaoEstadual = dto.InscricaoEstadual;
            entity.InscricaoMunicipal = dto.InscricaoMunicipal;
            entity.Ativo = dto.Ativo;
        }
    }
}
