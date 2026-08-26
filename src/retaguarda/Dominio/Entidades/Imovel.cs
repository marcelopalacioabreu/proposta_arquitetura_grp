using Retaguarda.Dominio.Entidades.Base;

namespace Retaguarda.Dominio.Entidades
{
    public class Imovel : MultilocatarioEntidade
    {
        public string Cadastro { get; set; } = string.Empty; // cadastro imobiliario
        public Endereco? Endereco { get; set; }        
        public string InscricaoImobiliaria { get; set; } = string.Empty;
        public long? TipoImovelId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? SituacaoId { get; set; }
    }
}
