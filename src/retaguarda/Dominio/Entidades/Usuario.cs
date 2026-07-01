using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Usuario : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        // Authentication fields
        public string Username { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string? Email { get; set; }

        public Organizacao? Organizacao { get; set; }
    }
}
