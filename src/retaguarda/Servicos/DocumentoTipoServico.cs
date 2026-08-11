using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class DocumentoTipoServico : ServicoBase<DocumentoTipo, DocumentoTipoDto>, IDocumentoTipoServico
    {
        private readonly IDocumentoTipoRepositorio _repositorioConcrete;

        public DocumentoTipoServico(IDocumentoTipoRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override DocumentoTipoDto ToDto(DocumentoTipo e)
        {
            return new DocumentoTipoDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Nome = e.Nome,
                Ativo = e.Ativo
            };
        }

        protected override DocumentoTipo FromDto(DocumentoTipoDto dto)
        {
            return new DocumentoTipo
            {
                Codigo = dto.Codigo,
                Nome = dto.Nome,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(DocumentoTipo entity, DocumentoTipoDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Nome = dto.Nome;
            entity.Ativo = dto.Ativo;
        }
    }
}
