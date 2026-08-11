using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class CepServico : ServicoBase<Cep, CepDto>, ICepServico
    {
        private readonly ICepRepositorio _repositorioConcrete;

        public CepServico(ICepRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override CepDto ToDto(Cep e)
        {
            return new CepDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                ImovelId = e.ImovelId,
                Ativo = e.Ativo
            };
        }

        protected override Cep FromDto(CepDto dto)
        {
            return new Cep
            {
                Codigo = dto.Codigo,
                ImovelId = dto.ImovelId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(Cep entity, CepDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.ImovelId = dto.ImovelId;
            entity.Ativo = dto.Ativo;
        }
    }
}
