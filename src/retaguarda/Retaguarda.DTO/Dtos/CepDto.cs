namespace Retaguarda.DTO.Dtos
{
    public class CepDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;

        public EnderecoLogradouroDto Logradouro { get; set; } = null!;
    }
}
