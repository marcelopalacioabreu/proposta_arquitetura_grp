using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class ImovelServico : ServicoBase<Imovel, ImovelDto>, IImovelServico
    {
        private readonly IImovelRepositorio _repositorioConcrete;

        public ImovelServico(IImovelRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override ImovelDto ToDto(Imovel e)
        {
            return new ImovelDto
            {
                Id = e.Id,
                Cadastro = e.Cadastro,
                LogradouroId = e.LogradouroId,
                CepId = e.CepId,
                Numero = e.Numero,
                Complemento = e.Complemento,
                InscricaoImobiliaria = e.InscricaoImobiliaria,
                TipoImovelId = e.TipoImovelId,
                Latitude = e.Latitude,
                Longitude = e.Longitude,
                SituacaoId = e.SituacaoId,
                Ativo = e.Ativo
            };
        }

        protected override Imovel FromDto(ImovelDto dto)
        {
            return new Imovel
            {
                Cadastro = dto.Cadastro,
                LogradouroId = dto.LogradouroId,
                CepId = dto.CepId,
                Numero = dto.Numero,
                Complemento = dto.Complemento,
                InscricaoImobiliaria = dto.InscricaoImobiliaria,
                TipoImovelId = dto.TipoImovelId,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                SituacaoId = dto.SituacaoId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Imovel entity, ImovelDto dto)
        {
            entity.Cadastro = dto.Cadastro;
            entity.LogradouroId = dto.LogradouroId;
            entity.CepId = dto.CepId;
            entity.Numero = dto.Numero;
            entity.Complemento = dto.Complemento;
            entity.InscricaoImobiliaria = dto.InscricaoImobiliaria;
            entity.TipoImovelId = dto.TipoImovelId;
            entity.Latitude = dto.Latitude;
            entity.Longitude = dto.Longitude;
            entity.SituacaoId = dto.SituacaoId;
            entity.Ativo = dto.Ativo;
        }
    }
}
