namespace Retaguarda.DTO.Dtos
{
    public class TipoEnderecoDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
