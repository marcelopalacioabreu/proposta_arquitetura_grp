using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Imovel : MultilocatarioEntidade
    {
        public string Cadastro { get; set; } = string.Empty; // cadastro imobiliario
        public long LogradouroId { get; set; }
        public Logradouro? Logradouro { get; set; }
    }
}
