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
    }
}
