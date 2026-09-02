namespace Retaguarda.DTO.Dtos
{
    public class OrganizacaoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public long? PessoaId { get; set; }
        public long? TipoId { get; set; }
        public long? SituacaoId { get; set; }
        public long? OrganizacaoPaiId { get; set; }
        public long? OrganizacaoRaizId { get; set; }
        public long? Nivel { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
