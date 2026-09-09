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
    public class CepServico : ServicoBase<EnderecoCEP, CepDto>, ICepServico
    {
        private readonly ICepRepositorio _repositorioConcrete;

        public CepServico(ICepRepositorio repositorio) : base(repositorio)
        {
            _repositorioConcrete = repositorio;
        }

        /// <summary>
        /// Override para carregar com Logradouro na listagem
        /// </summary>
        public override async Task<(List<CepDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, IDictionary<string, string>? filtros = null, int? inativo = null)
        {
            var (items, total) = await _repositorioConcrete.ListarComLogradouroAsync(nomeFilter, page, pageSize, sortField, sortDir, filtros, inativo);
            var dtos = items.Select(ToDto).ToList();
            return (dtos, total);
        }

        public override async Task<(List<CepDto> Items, int Total)> ListarAsync(PesquisaParametrosDto parametros)
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
                } : null,
                LogradouroNome = e.Logradouro?.Nome
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
