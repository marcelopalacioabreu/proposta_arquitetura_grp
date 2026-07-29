using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Perfil : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public Organizacao? Organizacao { get; set; }
        // If true, a user with this profile can perform any action in the system
        public bool AdministradorDoSistema { get; set; } = false;

        public ICollection<PerfilPermissao> Permissoes { get; set; } = new List<PerfilPermissao>();
    }
}
