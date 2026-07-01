using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Funcao : AuditableEntity
    {
        public long OrganizacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Organizacao? Organizacao { get; set; }
    }
}
