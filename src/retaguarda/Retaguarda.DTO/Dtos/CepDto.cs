namespace Retaguarda.DTO.Dtos
{
    public class CepDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public long? ImovelId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
