using Retaguarda.Dominio.Entidades.Base;
using System;

namespace Retaguarda.Dominio.Entidades
{
    public class OrquestracaoFluxoProcesso : MultilocatarioEntidade
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        // Elsa workflow linkage
        public string? WorkflowDefinitionId { get; set; }
        public int? WorkflowVersion { get; set; }
        
        // Armazenar JSON do workflow para listagem rápida e contexto
        public string? WorkflowJson { get; set; }
        
        // Nome do workflow no ElsaStudio (por referência/cache)
        public string? WorkflowNome { get; set; }
    }
}
