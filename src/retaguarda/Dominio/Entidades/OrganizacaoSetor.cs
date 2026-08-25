using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class OrganizacaoSetor : MultilocatarioEntidade
    {
        public string CodigoHierarquico { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public long? ResponsavelSetorId { get; set; }
    }
}
