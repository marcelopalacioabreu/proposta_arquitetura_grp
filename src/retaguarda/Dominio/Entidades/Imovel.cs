using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Imovel : MultilocatarioEntidade
    {
        public string Cadastro { get; set; } = string.Empty; // cadastro imobiliario
        public long? LogradouroId { get; set; }
        public Logradouro? Logradouro { get; set; }

        // additional fields
        public long? CepId { get; set; }
        public Cep? Cep { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string InscricaoImobiliaria { get; set; } = string.Empty;
        public long? TipoImovelId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? SituacaoId { get; set; }
    }
}
