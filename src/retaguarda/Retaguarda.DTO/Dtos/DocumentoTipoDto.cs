namespace Retaguarda.DTO.Dtos
{
    public class DocumentoTipoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
