using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class PerfilPermissao : AuditableEntity
    {
        public long OrganizacaoId { get; set; }
        public long PerfilId { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Perfil? Perfil { get; set; }
        public Organizacao? Organizacao { get; set; }
    }
}
