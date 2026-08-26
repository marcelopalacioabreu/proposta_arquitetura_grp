using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class LogradouroServico : ServicoBase<EnderecoLogradouro, EnderecoLogradouroDto>, ILogradouroServico
    {
        private readonly ILogradouroRepositorio _repositorioConcrete;

        public LogradouroServico(ILogradouroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override EnderecoLogradouroDto ToDto(EnderecoLogradouro e)
        {
            return new EnderecoLogradouroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Tipo = e.Tipo,
                BairroId = e.BairroId,
                Ativo = e.Ativo
            };
        }

        protected override EnderecoLogradouro FromDto(EnderecoLogradouroDto dto)
        {
            return new EnderecoLogradouro
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                BairroId = dto.BairroId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoLogradouro entity, EnderecoLogradouroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Tipo = dto.Tipo;
            entity.BairroId = dto.BairroId;
            entity.Ativo = dto.Ativo;
        }
    }
}
