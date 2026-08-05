using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Cep : MultilocatarioEntidade
    {
        public string Codigo { get; set; } = string.Empty;
        // CEP may not be linked to an Imovel
        public long? ImovelId { get; set; }
        public Imovel? Imovel { get; set; }
    }
}
