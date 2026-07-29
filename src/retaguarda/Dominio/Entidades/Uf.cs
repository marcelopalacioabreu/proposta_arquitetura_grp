using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Uf : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;

        public long PaisId { get; set; }
        public Pais? Pais { get; set; }
    }
}
