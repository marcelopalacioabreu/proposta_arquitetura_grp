using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class LogradouroServico : ServicoBase<Logradouro, LogradouroDto>, ILogradouroServico
    {
        private readonly ILogradouroRepositorio _repositorioConcrete;

        public LogradouroServico(ILogradouroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override LogradouroDto ToDto(Logradouro e)
        {
            return new LogradouroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Tipo = e.Tipo,
                BairroId = e.BairroId,
                Ativo = e.Ativo
            };
        }

        protected override Logradouro FromDto(LogradouroDto dto)
        {
            return new Logradouro
            {
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                BairroId = dto.BairroId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Logradouro entity, LogradouroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Tipo = dto.Tipo;
            entity.BairroId = dto.BairroId;
            entity.Ativo = dto.Ativo;
        }
    }
}
