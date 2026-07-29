using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Pais : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty; // ISO code
    }
}
