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

        // TODO: PessoaServico needs refactoring - Pessoa is now abstract base class
        // Operations should be performed on PessoaFisica or PessoaJuridica directly
        protected override PessoaDto ToDto(Pessoa e)
        {
            return new PessoaDto
            {
                Id = e.Id,
                TipoPessoa = e.TipoPessoa.Chave,
                Ativo = e.Ativo
            };
        }

        protected override Pessoa FromDto(PessoaDto dto)
        {
            // Nao instancia a classe abstrata diretamente
            throw new System.NotImplementedException("Use PessoaFisicaServico ou PessoaJuridicaServico");
        }

        protected override void UpdateEntityFromDto(Pessoa entity, PessoaDto dto)
        {
            // Nao atualiza a classe abstrata diretamente
            throw new System.NotImplementedException("Use PessoaFisicaServico ou PessoaJuridicaServico");
        }
    }
}
