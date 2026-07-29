using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Cep : MultilocatarioEntidade
    {
        public string Codigo { get; set; } = string.Empty;
        public long ImovelId { get; set; }
        public Imovel? Imovel { get; set; }
    }
}
