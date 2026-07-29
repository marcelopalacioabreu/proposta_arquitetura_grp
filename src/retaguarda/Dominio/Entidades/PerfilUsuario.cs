using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    // Association between Usuario and Perfil
    public class PerfilUsuario : MultilocatarioEntidade
    {
        public long UsuarioId { get; set; }
        public long PerfilId { get; set; }

        public Usuario? Usuario { get; set; }
        public Perfil? Perfil { get; set; }
    }
}
