using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
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
            return new PessoaDto
            {
                Id = e.Id,
                Nome = e.Nome,
                NomeSocial = e.NomeSocial,
                Cpf = e.Cpf,
                DataNascimento = e.DataNascimento,
                SexoId = e.SexoId,
                EstadoCivilId = e.EstadoCivilId,
                NacionalidadePaisId = e.NacionalidadePaisId,
                NaturalidadeMunicipioId = e.NaturalidadeMunicipioId,
                NomeMae = e.NomeMae,
                NomePai = e.NomePai,
                Pcd = e.Pcd,
                DataObito = e.DataObito,
                TipoPessoaChave = e.TipoPessoaChave,
                Documento = e.Documento,
                Telefone = e.Telefone,
                Ativo = e.Ativo
            };
        }

        protected override Pessoa FromDto(PessoaDto dto)
        {
            return new Pessoa
            {
                Nome = dto.Nome,
                NomeSocial = dto.NomeSocial,
                Cpf = dto.Cpf,
                DataNascimento = dto.DataNascimento,
                SexoId = dto.SexoId,
                EstadoCivilId = dto.EstadoCivilId,
                NacionalidadePaisId = dto.NacionalidadePaisId,
                NaturalidadeMunicipioId = dto.NaturalidadeMunicipioId,
                NomeMae = dto.NomeMae,
                NomePai = dto.NomePai,
                Pcd = dto.Pcd,
                DataObito = dto.DataObito,
                TipoPessoaChave = dto.TipoPessoaChave,
                Documento = dto.Documento,
                Telefone = dto.Telefone,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Pessoa entity, PessoaDto dto)
        {
            entity.Nome = dto.Nome;
            entity.NomeSocial = dto.NomeSocial;
            entity.Cpf = dto.Cpf;
            entity.DataNascimento = dto.DataNascimento;
            entity.SexoId = dto.SexoId;
            entity.EstadoCivilId = dto.EstadoCivilId;
            entity.NacionalidadePaisId = dto.NacionalidadePaisId;
            entity.NaturalidadeMunicipioId = dto.NaturalidadeMunicipioId;
            entity.NomeMae = dto.NomeMae;
            entity.NomePai = dto.NomePai;
            entity.Pcd = dto.Pcd;
            entity.DataObito = dto.DataObito;
            entity.TipoPessoaChave = dto.TipoPessoaChave;
            entity.Documento = dto.Documento;
            entity.Telefone = dto.Telefone;
            entity.Ativo = dto.Ativo;
        }
    }
}
