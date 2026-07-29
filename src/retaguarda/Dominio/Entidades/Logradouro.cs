using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Logradouro : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Rua";

        public long BairroId { get; set; }
        public Bairro? Bairro { get; set; }
    }
}
