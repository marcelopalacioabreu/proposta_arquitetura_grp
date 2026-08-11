namespace Retaguarda.DTO.Dtos
{
    public class UfDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long PaisId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
