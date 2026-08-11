using System.Collections.Generic;

namespace Retaguarda.DTO.Parametros
{
    public class PesquisaParametrosDto
    {
        public string? Nome { get; set; }
        public int Pagina { get; set; } = 1;
        public int TamanhoPagina { get; set; } = 10;
        public string? SortField { get; set; }
        public string? SortDir { get; set; }
        public System.Collections.Generic.Dictionary<string,string>? Filtros { get; set; }
        public int? Inativo { get; set; }
    }
}
