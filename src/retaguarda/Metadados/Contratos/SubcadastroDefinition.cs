using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Retaguarda.Metadados.Contracts
{
    /// <summary>
    /// Define um subcadastro (subtabela) para associação de entidades relacionadas
    /// Exemplo: Setores e Unidades de atuação de um usuário
    /// </summary>
    public class SubcadastroDefinition
    {
        /// <summary>Nome único do subcadastro para rastreamento de eventos</summary>
        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        /// <summary>Título exibido na UI</summary>
        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        /// <summary>Endpoint para listar opções disponíveis</summary>
        [JsonPropertyName("endpoint")]
        public string? Endpoint { get; set; }

        /// <summary>Propriedade que armazena o ID da linha na tela</summary>
        [JsonPropertyName("chaveLocal")]
        public string? ChaveLocal { get; set; } = "id";

        /// <summary>Colunas da subtabela</summary>
        [JsonPropertyName("colunas")]
        public List<SubcadastroColunaDefinition>? Colunas { get; set; }

        /// <summary>Configurações de seleção</summary>
        [JsonPropertyName("selecao")]
        public SubcadastroSelecaoDefinition? Selecao { get; set; }

        /// <summary>Campo que armazena dados da subtabela no formulário principal</summary>
        [JsonPropertyName("campoArmazenamento")]
        public string? CampoArmazenamento { get; set; }

        /// <summary>Máximo de linhas permitidas (nulo = ilimitado)</summary>
        [JsonPropertyName("maxLinhas")]
        public int? MaxLinhas { get; set; }
    }

    /// <summary>Definição de coluna em um subcadastro</summary>
    public class SubcadastroColunaDefinition
    {
        /// <summary>Nome do campo na entidade</summary>
        [JsonPropertyName("campo")]
        public string? Campo { get; set; }

        /// <summary>Rótulo exibido no cabeçalho</summary>
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        /// <summary>Tipo de controle: text, select, number, checkbox, date, etc</summary>
        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        /// <summary>Largura em colunas (1-12 em grid 12 colunas)</summary>
        [JsonPropertyName("col")]
        public int? Col { get; set; }

        /// <summary>Endpoint ou chave de enumeração para opções de select</summary>
        [JsonPropertyName("enumeracao")]
        public string? Enumeracao { get; set; }

        /// <summary>Indica que a coluna é somente leitura</summary>
        [JsonPropertyName("readonly")]
        public bool? Readonly { get; set; }

        /// <summary>Dica de ajuda</summary>
        [JsonPropertyName("placeholder")]
        public string? Placeholder { get; set; }
    }

    /// <summary>Configuração de seleção no subcadastro</summary>
    public class SubcadastroSelecaoDefinition
    {
        /// <summary>Campo para checkbox de seleção/padrão</summary>
        [JsonPropertyName("campo")]
        public string? Campo { get; set; }

        /// <summary>Rótulo do checkbox</summary>
        [JsonPropertyName("label")]
        public string? Label { get; set; }

        /// <summary>Permite apenas uma linha marcada (radio vs checkbox)</summary>
        [JsonPropertyName("singleSelecao")]
        public bool? SingleSelecao { get; set; }

        /// <summary>Dados dessa linha serão mergeados no formulário principal ao salvar</summary>
        [JsonPropertyName("mergeNoPrincipal")]
        public bool? MergeNoPrincipal { get; set; }
    }
}
