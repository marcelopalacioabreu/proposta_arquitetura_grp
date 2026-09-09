using Retaguarda.Dominio.Entidades;

namespace Retaguarda.Repositorios.Interfaces
{
    public interface ITipoRepositorio : IRepositorioBase<Tipo>
    {
        Task<(List<Tipo> Items, int Total)> ListarPorContextoAsync(
            string? filtroNome, 
            string? contexto,
            int page = 1, 
            int pageSize = 10, 
            string? sortField = null, 
            string? sortDir = null, 
            Dictionary<string, object>? filtros = null, 
            bool inativo = false);
    }
}
