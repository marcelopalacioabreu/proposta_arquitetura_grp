namespace Retaguarda.DTO.Dtos
{
    public class EnderecoPaisDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
