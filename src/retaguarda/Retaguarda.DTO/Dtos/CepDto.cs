namespace Retaguarda.DTO.Dtos
{
    public class CepDto
    {
        public long Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public long LogradouroId { get; set; }
        public bool Ativo { get; set; } = true;

        // Propriedade de navegação para leitura (carregada sob demanda)
        public EnderecoLogradouroDto? Logradouro { get; set; }
        
        /// <summary>
        /// Campo flatteado para exibição do nome do logradouro na tela de pesquisa
        /// </summary>
        public string? LogradouroNome { get; set; }
    }
}
