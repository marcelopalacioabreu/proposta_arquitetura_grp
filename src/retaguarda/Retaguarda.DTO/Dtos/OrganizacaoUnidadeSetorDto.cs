namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoUnidadeSetorDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public long? OrganizacaoUnidadeId { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
