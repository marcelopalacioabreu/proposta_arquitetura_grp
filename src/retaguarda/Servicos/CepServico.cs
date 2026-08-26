using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;

namespace Retaguarda.Servicos
{
    public class CepServico : ServicoBase<EnderecoCEP, CepDto>, ICepServico
    {
        private readonly ICepRepositorio _repositorioConcrete;

        public CepServico(ICepRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        protected override CepDto ToDto(EnderecoCEP e)
        {
            return new CepDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                Ativo = e.Ativo
            };
        }

        protected override EnderecoCEP FromDto(CepDto dto)
        {
            return new EnderecoCEP
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoCEP entity, CepDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.Ativo = dto.Ativo;
        }
    }
}
