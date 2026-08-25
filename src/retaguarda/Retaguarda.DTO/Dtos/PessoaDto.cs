namespace Retaguarda.DTO.Dtos
{
    public class PessoaDto
    {
        public long Id { get; set; }
        public int TipoPessoa { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
