namespace Retaguarda.DTO.Dtos
{
    public class EnderecoBairroDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public long MunicipioId { get; set; }
        public bool Ativo { get; set; } = true;

        public EnderecoMunicipioDto? Municipio { get; set; }
        
        /// <summary>
        /// Campo flatteado para exibição do nome do município na tela de pesquisa
        /// </summary>
        public string? MunicipioNome { get; set; }
    }
}
