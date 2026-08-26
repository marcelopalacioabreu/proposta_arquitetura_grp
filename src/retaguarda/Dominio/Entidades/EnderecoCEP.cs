using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class EnderecoCEP : MultilocatarioEntidade
    {
        public string Codigo { get; set; } = string.Empty;
        public EnderecoLogradouro Logradouro { get; set; } = null!;
    }
}
