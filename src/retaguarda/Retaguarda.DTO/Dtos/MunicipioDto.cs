namespace Retaguarda.DTO.Dtos
{
    public class MunicipioDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CodigoIbge { get; set; } = string.Empty;
        public long UfId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
