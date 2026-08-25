using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoUnidade : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? UnidadePaiId { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
        public string HierarquiaNome { get; set; } = string.Empty;
        public long? Nivel { get; set; }
    }
}
