namespace Retaguarda.DTO.Dtos
{
    public class EnderecoMunicipioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoIbge { get; set; } = string.Empty;
        public long UfId { get; set; }
        public bool Ativo { get; set; } = true;

        public EnderecoUFDto Uf { get; set; } = null!;
    }
}
