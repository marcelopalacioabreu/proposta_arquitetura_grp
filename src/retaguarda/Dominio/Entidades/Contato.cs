using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Contato : MultilocatarioEntidade
    {
        public long? TipoContatoId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string ContatoValor { get; set; } = string.Empty;
    }
}
