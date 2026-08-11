namespace Retaguarda.DTO.Dtos
{
    public class EnderecoDto
    {
        public long Id { get; set; }
        public long UsuarioId { get; set; }
        public long CepId { get; set; }
        public string Complemento { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
