using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class EnderecoBairro : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;

        public long MunicipioId { get; set; }
        public EnderecoMunicipio? Municipio { get; set; }
    }
}
