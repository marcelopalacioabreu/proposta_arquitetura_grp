using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class EnderecoMunicipio : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string CodigoIbge { get; set; } = string.Empty;

        public long UfId { get; set; }
        public long? CepId { get; set; }
        public EnderecoUF? Uf { get; set; }
    }
}
