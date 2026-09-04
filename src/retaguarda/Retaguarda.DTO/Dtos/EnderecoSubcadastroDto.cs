namespace Retaguarda.DTO.Dtos
{
    public class EnderecoSubcadastroDto
    {
        public long? Id { get; set; }        // join-table row id (PessoaEndereco.Id etc.)
        public long? EnderecoId { get; set; } // Endereco.Id — null for new rows
        public long? CepId { get; set; }
        public string? Complemento { get; set; }
        public long? TipoId { get; set; }    // EnderecoTipoId
        public bool Principal { get; set; }
    }
}
