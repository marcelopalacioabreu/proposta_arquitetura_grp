using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Organizacao : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? ResponsavelId { get; set; }
        public long? SituacaoId { get; set; }
        public long? NivelGovernoId { get; set; }
        public long? OrganizacaoPaiId { get; set; }
        public long? OrganizacaoRaizId { get; set; }
        public string HierarquiaCodigo { get; set; } = string.Empty;
    }
}
