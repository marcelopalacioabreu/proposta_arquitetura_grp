namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoUnidadeSetorDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }
}
