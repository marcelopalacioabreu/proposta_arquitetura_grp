namespace Retaguarda.DTO.Dtos
{
    public class OrquestracaoFluxoProcessoDto
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? WorkflowDefinitionId { get; set; }
        public int? WorkflowVersion { get; set; }
        public bool Ativo { get; set; } = true;
        
        // Novos campos para armazenar workflow JSON e referência
        public string? WorkflowJson { get; set; }
        public string? WorkflowNome { get; set; }
    }
}
