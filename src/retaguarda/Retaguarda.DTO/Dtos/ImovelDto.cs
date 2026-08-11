namespace Retaguarda.DTO.Dtos
{
    public class ImovelDto
    {
        public long Id { get; set; }
        public string Cadastro { get; set; } = string.Empty;
        public long? LogradouroId { get; set; }
        public long? CepId { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string InscricaoImobiliaria { get; set; } = string.Empty;
        public long? TipoImovelId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public long? SituacaoId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
