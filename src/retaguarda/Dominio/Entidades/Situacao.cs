using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Situacao : MultilocatarioEntidade
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Contexto { get; set; } = string.Empty; // IMOVEL
        public string Descricao { get; set; } = string.Empty;
    }
}
