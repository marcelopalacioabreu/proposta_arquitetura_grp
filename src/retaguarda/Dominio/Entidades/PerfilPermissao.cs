using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class PerfilPermissao : MultilocatarioEntidade
    {
        public long PerfilId { get; set; }
        public string Nome { get; set; } = string.Empty;

        public Perfil? Perfil { get; set; }
        public Organizacao? Organizacao { get; set; }
    }
}
