using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class EnderecoUF : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;

        public long PaisId { get; set; }
        public EnderecoPais? Pais { get; set; }
    }
}
