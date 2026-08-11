namespace Retaguarda.DTO.Dtos
{
    public class BairroDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public long MunicipioId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
