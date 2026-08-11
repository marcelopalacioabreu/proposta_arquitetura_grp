using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class TipoContatoServico : ServicoBase<TipoContato, TipoContatoDto>, ITipoContatoServico
    {
        private readonly ITipoContatoRepositorio _repositorioConcrete;

        public TipoContatoServico(ITipoContatoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override TipoContatoDto ToDto(TipoContato e)
        {
            return new TipoContatoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override TipoContato FromDto(TipoContatoDto dto)
        {
            return new TipoContato
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(TipoContato entity, TipoContatoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
