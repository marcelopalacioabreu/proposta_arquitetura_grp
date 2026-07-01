using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Perfil : AuditableEntity
    {
        public long OrganizacaoId { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Organizacao? Organizacao { get; set; }
        public ICollection<PerfilPermissao> Permissoes { get; set; } = new List<PerfilPermissao>();
    }
}
