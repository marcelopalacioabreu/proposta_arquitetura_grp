using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class TipoEnderecoServico : ServicoBase<TipoEndereco, TipoEnderecoDto>, ITipoEnderecoServico
    {
        private readonly ITipoEnderecoRepositorio _repositorioConcrete;

        public TipoEnderecoServico(ITipoEnderecoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override TipoEnderecoDto ToDto(TipoEndereco e)
        {
            return new TipoEnderecoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override TipoEndereco FromDto(TipoEnderecoDto dto)
        {
            return new TipoEndereco
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(TipoEndereco entity, TipoEnderecoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
