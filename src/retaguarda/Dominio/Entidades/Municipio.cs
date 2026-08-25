using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Municipio : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string CodigoIbge { get; set; } = string.Empty;

        public long UfId { get; set; }
        public long? CepId { get; set; }
        public Uf? Uf { get; set; }
    }
}
