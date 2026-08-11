using System.Collections.Generic;
using System.Threading.Tasks;
using Retaguarda.Repositorios.Interfaces;
using Retaguarda.Servicos.Interfaces;

namespace Retaguarda.Servicos.Base
{
    public abstract class ServicoBase<TEntity, TDto> : IServicoBase<TDto> where TEntity : class
    {
        protected readonly IRepositorioBase<TEntity> _repositorio;

        protected ServicoBase(IRepositorioBase<TEntity> repositorio)
        {
            _repositorio = repositorio;
        }

        protected abstract TDto ToDto(TEntity e);
        protected abstract TEntity FromDto(TDto dto);
        protected abstract void UpdateEntityFromDto(TEntity entity, TDto dto);

        public virtual async Task<TDto?> ObterPorIdAsync(long id)
        {
            var e = await _repositorio.ObterPorIdAsync(id);
            return e == null ? default : ToDto(e);
        }

        public virtual async Task<(List<TDto> Items, int Total)> ListarAsync(string? nomeFilter, int page, int pageSize, string? sortField, string? sortDir, System.Collections.Generic.IDictionary<string,string>? filtros = null, int? inativo = null)
        {
            var (items, total) = await _repositorio.ListarAsync(nomeFilter, page, pageSize, sortField, sortDir, filtros, inativo);
            var dtos = new List<TDto>();
            foreach (var it in items) dtos.Add(ToDto(it));
            return (dtos, total);
        }

        public virtual async Task<(List<TDto> Items, int Total)> ListarAsync(Retaguarda.DTO.Parametros.PesquisaParametrosDto parametros)
        {
            var (items, total) = await _repositorio.ListarAsync(parametros);
            var dtos = new List<TDto>();
            foreach (var it in items) dtos.Add(ToDto(it));
            return (dtos, total);
        }

        public virtual async Task<TDto> CriarAsync(TDto dto)
        {
            var e = FromDto(dto);
            var added = await _repositorio.AdicionarAsync(e);
            return ToDto(added);
        }

        public virtual async Task UpdateAsync(long id, TDto dto)
        {
            var e = await _repositorio.ObterPorIdAsync(id);
            if (e == null) return;
            UpdateEntityFromDto(e, dto);
            await _repositorio.UpdateAsync(e);
        }

        public virtual async Task DeleteAsync(long id) => await _repositorio.DeleteAsync(id);

        public virtual async Task RestaurarAsync(long id) => await _repositorio.RestaurarAsync(id);
    }
}
