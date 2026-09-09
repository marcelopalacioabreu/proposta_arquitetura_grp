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
    public class LogradouroServico : ServicoBase<EnderecoLogradouro, EnderecoLogradouroDto>, ILogradouroServico
    {
        private readonly ILogradouroRepositorio _repositorioConcrete;

        public LogradouroServico(ILogradouroRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        /// <summary>
        /// Override para carregar com Bairro na listagem
        /// </summary>
        public override async Task<(List<EnderecoLogradouroDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var (items, total) = await _repositorioConcrete.ListarComBairroAsync(nomeFilter, page, pageSize, sortField, sortDir, filtros, inativo);
            var dtos = items.Select(ToDto).ToList();
            return (dtos, total);
        }

        public override async Task<(List<EnderecoLogradouroDto> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros)
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

        protected override EnderecoLogradouroDto ToDto(EnderecoLogradouro e)
        {
            return new EnderecoLogradouroDto
            {
                Id = e.Id,
                Nome = e.Nome,
                Tipo = e.Tipo,
                BairroId = e.BairroId,
                Ativo = e.Ativo,
                Bairro = e.Bairro != null ? new EnderecoBairroDto
                {
                    Id = e.Bairro.Id,
                    Nome = e.Bairro.Nome,
                    MunicipioId = e.Bairro.MunicipioId,
                    Ativo = e.Bairro.Ativo
                } : null,
                BairroNome = e.Bairro?.Nome
            };
        }

        protected override EnderecoLogradouro FromDto(EnderecoLogradouroDto dto)
        {
            return new EnderecoLogradouro
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Tipo = dto.Tipo,
                BairroId = dto.BairroId,
                Ativo = dto.Ativo
            };
        }

        protected override void UpdateEntityFromDto(EnderecoLogradouro entity, EnderecoLogradouroDto dto)
        {
            entity.Nome = dto.Nome;
            entity.Tipo = dto.Tipo;
            entity.BairroId = dto.BairroId;
            entity.Ativo = dto.Ativo;
        }
    }
}
