namespace Retaguarda.DTO.Dtos
{
    public class PessoaDto
    {
        public long Id { get; set; }
        public string TipoPessoa { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
