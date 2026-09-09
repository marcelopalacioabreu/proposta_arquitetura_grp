using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Retaguarda.Dominio.Entidades;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Base;
using Retaguarda.Servicos.Interfaces;
using Retaguarda.DTO.Dtos;
using Retaguarda.DTO.Parametros;

namespace Retaguarda.Servicos
{
    public class BairroServico : ServicoBase<EnderecoBairro, EnderecoBairroDto>, IBairroServico
    {
        private readonly IBairroRepositorio _repositorioConcrete;

        public BairroServico(IBairroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        /// <summary>
        /// Override para carregar com Municipio na listagem
        /// </summary>
        public override async Task<(List<EnderecoBairroDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var (items, total) = await _repositorioConcrete.ListarComMunicipioAsync(nomeFilter, page, pageSize, sortField, sortDir, filtros, inativo);
            var dtos = items.Select(ToDto).ToList();
            return (dtos, total);
        }

        public override async Task<(List<EnderecoBairroDto> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros)
        {
            var nome = parametros?.Nome;
            var page = parametros?.Pagina ?? 1;
            var pageSize = parametros?.TamanhoPagina ?? 10;
            var sortField = parametros?.SortField;
            var sortDir = parametros?.SortDir;
            var filtros = parametros?.Filtros;
            var inativo = parametros?.Inativo;
            return await ListarAsync(nome, page, pageSize, sortField, sortDir, filtros, inativo);
        }

        protected override EnderecoBairroDto ToDto(EnderecoBairro e)
        {
            return new EnderecoBairroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                MunicipioId = e.MunicipioId,
                Ativo = e.Ativo,
                Municipio = e.Municipio != null ? new EnderecoMunicipioDto
                {
                    Id = e.Municipio.Id,
                    Nome = e.Municipio.Nome,
                    CodigoIbge = e.Municipio.CodigoIbge,
                    UfId = e.Municipio.UfId,
                    Ativo = e.Municipio.Ativo
                } : null,
                MunicipioNome = e.Municipio?.Nome
            };
        }

        protected override EnderecoBairro FromDto(EnderecoBairroDto dto)
        {
            return new EnderecoBairro
            {
                Id = dto.Id,
                Nome = dto.Nome,
                MunicipioId = dto.MunicipioId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoBairro entity, EnderecoBairroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.MunicipioId = dto.MunicipioId;
            entity.Ativo = dto.Ativo;
        }
    }
}
