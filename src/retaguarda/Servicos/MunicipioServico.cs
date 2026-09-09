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
    public class MunicipioServico : ServicoBase<EnderecoMunicipio, EnderecoMunicipioDto>, IMunicipioServico
    {
        private readonly IMunicipioRepositorio _repositorioConcrete;

        public MunicipioServico(IMunicipioRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        /// <summary>
        /// Override para carregar com UF na listagem
        /// </summary>
        public override async Task<(List<EnderecoMunicipioDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var (items, total) = await _repositorioConcrete.ListarComUfAsync(nomeFilter, page, pageSize, sortField, sortDir, filtros, inativo);
            var dtos = items.Select(ToDto).ToList();
            return (dtos, total);
        }

        public override async Task<(List<EnderecoMunicipioDto> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros)
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

        protected override EnderecoMunicipioDto ToDto(EnderecoMunicipio e)
        {
            return new EnderecoMunicipioDto
            {
                Id = e.Id,
                Nome = e.Nome,
                CodigoIbge = e.CodigoIbge,
                UfId = e.UfId,
                Ativo = e.Ativo,
                Uf = e.Uf != null ? new EnderecoUFDto
                {
                    Id = e.Uf.Id,
                    Nome = e.Uf.Nome,
                    Sigla = e.Uf.Sigla,
                    Ativo = e.Uf.Ativo
                } : null,
                UfSigla = e.Uf?.Sigla
            };
        }

        protected override EnderecoMunicipio FromDto(EnderecoMunicipioDto dto)
        {
            return new EnderecoMunicipio
            {
                Id = dto.Id,
                Nome = dto.Nome,
                CodigoIbge = dto.CodigoIbge,
                UfId = dto.UfId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoMunicipio entity, EnderecoMunicipioDto dto)
        {
            entity.Nome = dto.Nome;
            entity.CodigoIbge = dto.CodigoIbge;
            entity.UfId = dto.UfId;
            entity.Ativo = dto.Ativo;
        }
    }
}
