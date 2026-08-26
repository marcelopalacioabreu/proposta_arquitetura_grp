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
    public class PessoaFisicaServico : ServicoBase<PessoaFisica, PessoaFisicaDto>, IPessoaFisicaServico
    {
        private readonly IRepositorioBase<Pessoa> _repositorioConcrete;

        public PessoaFisicaServico(IRepositorioBase<Pessoa> repositorio) : base((IRepositorioBase<PessoaFisica>)(object)repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override PessoaFisicaDto ToDto(PessoaFisica e)
        {
            return new PessoaFisicaDto
            {
                Id = e.Id,
                Nome = e.Nome,
                NomeSocial = e.NomeSocial,
                Cpf = e.Cpf,
                DataNascimento = e.DataNascimento,
                Sexo = e.Sexo != null ? e.Sexo.Chave : null,
                EstadoCivil = e.EstadoCivil != null ? e.EstadoCivil.Chave : null,
                NomeMae = e.NomeMae,
                NomePai = e.NomePai,
                Pcd = e.Pcd,
                DataObito = e.DataObito,
                Ativo = e.Ativo
            };
        }

        protected override PessoaFisica FromDto(PessoaFisicaDto dto)
        {
            return new PessoaFisica
            {
                Nome = dto.Nome,
                NomeSocial = dto.NomeSocial,
                Cpf = dto.Cpf,
                DataNascimento = dto.DataNascimento,
                Sexo = !string.IsNullOrEmpty(dto.Sexo) ? Sexo.ObterPorChave(dto.Sexo) : null,
                EstadoCivil = !string.IsNullOrEmpty(dto.EstadoCivil) ? EstadoCivil.ObterPorChave(dto.EstadoCivil) : null,
                NomeMae = dto.NomeMae,
                NomePai = dto.NomePai,
                Pcd = dto.Pcd,
                DataObito = dto.DataObito,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(PessoaFisica entity, PessoaFisicaDto dto)
        {
            entity.Nome = dto.Nome;
            entity.NomeSocial = dto.NomeSocial;
            entity.Cpf = dto.Cpf;
            entity.DataNascimento = dto.DataNascimento;
            entity.Sexo = !string.IsNullOrEmpty(dto.Sexo) ? Sexo.ObterPorChave(dto.Sexo) : null;
            entity.EstadoCivil = !string.IsNullOrEmpty(dto.EstadoCivil) ? EstadoCivil.ObterPorChave(dto.EstadoCivil) : null;
            entity.NomeMae = dto.NomeMae;
            entity.NomePai = dto.NomePai;
            entity.Pcd = dto.Pcd;
            entity.DataObito = dto.DataObito;
            entity.Ativo = dto.Ativo;
        }
    }
}
