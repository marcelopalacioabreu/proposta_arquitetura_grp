using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public enum TipoPessoa
    {
        FISICA = 1,
        JURIDICA = 2
    }

    public class Pessoa : MultilocatarioEntidade
    {
        public TipoPessoa TipoPessoa { get; set; }
    }
}
