namespace Retaguarda.DTO.Dtos
{
    public class EnderecoBairroDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public long MunicipioId { get; set; }
        public bool Ativo { get; set; } = true;

        public EnderecoMunicipioDto Municipio { get; set; } = null!;
    }
}
