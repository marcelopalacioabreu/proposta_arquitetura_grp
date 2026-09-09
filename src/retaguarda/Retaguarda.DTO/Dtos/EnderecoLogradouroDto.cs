namespace Retaguarda.DTO.Dtos
{
    public class EnderecoLogradouroDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Tipo { get; set; } = "Rua";
        public long BairroId { get; set; }
        public bool Ativo { get; set; } = true;

        public EnderecoBairroDto? Bairro { get; set; }
        
        /// <summary>
        /// Campo flatteado para exibição do nome do bairro na tela de pesquisa
        /// </summary>
        public string? BairroNome { get; set; }
    }
}
