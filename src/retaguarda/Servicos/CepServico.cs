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

        /// <summary>
        /// Obtém um CEP pelo código com todos os relacionamentos carregados
        /// </summary>
        public async Task<CepDto?> ObterPorCodigoComLogradouroAsync(string codigo)
        {
            var entity = await _repositorioConcrete.ObterPorCodigoComLogradouroAsync(codigo);
            return entity != null ? ToDto(entity) : null;
        }

        protected override CepDto ToDto(EnderecoCEP e)
        {
            return new CepDto
            {
                Id = e.Id,
                Codigo = e.Codigo,
                LogradouroId = e.LogradouroId,
                Ativo = e.Ativo,
                Logradouro = e.Logradouro != null ? new EnderecoLogradouroDto
                {
                    Id = e.Logradouro.Id,
                    Nome = e.Logradouro.Nome,
                    Tipo = e.Logradouro.Tipo,
                    BairroId = e.Logradouro.BairroId,
                    Ativo = e.Logradouro.Ativo
                } : null
            };
        }

        protected override EnderecoCEP FromDto(CepDto dto)
        {
            return new EnderecoCEP
            {
                Id = dto.Id,
                Codigo = dto.Codigo,
                LogradouroId = dto.LogradouroId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoCEP entity, CepDto dto)
        {
            entity.Codigo = dto.Codigo;
            entity.LogradouroId = dto.LogradouroId;
            entity.Ativo = dto.Ativo;
        }
    }
}
