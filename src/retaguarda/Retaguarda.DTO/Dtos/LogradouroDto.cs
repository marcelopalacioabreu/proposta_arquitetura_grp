namespace Retaguarda.DTO.Dtos
{
    public class LogradouroDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Rua";
        public long BairroId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
