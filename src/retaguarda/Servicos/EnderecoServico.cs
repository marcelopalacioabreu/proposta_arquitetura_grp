using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class EnderecoServico : ServicoBase<Endereco, EnderecoDto>, IEnderecoServico
    {
        private readonly IEnderecoRepositorio _repositorioConcrete;

        public EnderecoServico(IEnderecoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override EnderecoDto ToDto(Endereco e)
        {
            return new EnderecoDto
            {
                Id = e.Id,
                UsuarioId = e.UsuarioId,
                CepId = e.CepId,
                Complemento = e.Complemento,
                Ativo = e.Ativo
            };
        }

        protected override Endereco FromDto(EnderecoDto dto)
        {
            return new Endereco
            {
                UsuarioId = dto.UsuarioId,
                CepId = dto.CepId,
                Complemento = dto.Complemento,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Endereco entity, EnderecoDto dto)
        {
            entity.UsuarioId = dto.UsuarioId;
            entity.CepId = dto.CepId;
            entity.Complemento = dto.Complemento;
            entity.Ativo = dto.Ativo;
        }
    }
}
