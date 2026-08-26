using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class TipoServico : ServicoBase<Tipo, TipoDto>, ITipoServico
    {
        private readonly ITipoRepositorio _repositorioConcrete;

        public TipoServico(ITipoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override TipoDto ToDto(Tipo e)
        {
            return new TipoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Contexto = e.Contexto,
                Descricao = e.Descricao,
                Ativo = e.Ativo
            };
        }

        protected override Tipo FromDto(TipoDto dto)
        {
            return new Tipo
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Contexto = dto.Contexto,
                Descricao = dto.Descricao,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Tipo entity, TipoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Contexto = dto.Contexto;
            entity.Descricao = dto.Descricao;
            entity.Ativo = dto.Ativo;
        }
    }
}
