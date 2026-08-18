using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Retaguarda.Metadados.Contracts
{
    /// <summary>Definição de tela de cadastro, pesquisa ou outro tipo</summary>
    public class ScreenDefinition
    {
        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("extremidade")]
        public string? Extremidade { get; set; }

        [JsonPropertyName("endpoint")]
        public string? Endpoint { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("urlTela")]
        public string? UrlTela { get; set; }

        [JsonPropertyName("camposChaveUrl")]
        public List<string>? CamposChaveUrl { get; set; }

        [JsonPropertyName("itens")]
        public List<object>? Itens { get; set; }

        [JsonPropertyName("campos")]
        public List<FieldDefinition>? Campos { get; set; }

        [JsonPropertyName("filtro")]
        public List<FilterDefinition>? Filtro { get; set; }

        [JsonPropertyName("colunas")]
        public List<ColumnDefinition>? Colunas { get; set; }

        [JsonPropertyName("subcadastros")]
        public List<SubcadastroDefinition>? Subcadastros { get; set; }

        [JsonPropertyName("tabela")]
        public TableDefinition? Tabela { get; set; }
    }

    /// <summary>Definição de um campo em um formulário</summary>
    public class FieldDefinition
    {
        [JsonPropertyName("campo")]
        public string? Campo { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("col")]
        public int? Col { get; set; }

        [JsonPropertyName("columns")]
        public int? Columns { get; set; }

        [JsonPropertyName("titulo")]
        public string? Titulo { get; set; }

        [JsonPropertyName("campos")]
        public List<FieldDefinition>? Campos { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("endpoint")]
        public string? Endpoint { get; set; }

        [JsonPropertyName("extremidadeOpcoes")]
        public string? ExtremidadeOpcoes { get; set; }

        [JsonPropertyName("optionsEndpoint")]
        public string? OptionsEndpoint { get; set; }

        [JsonPropertyName("enumeracao")]
        public string? Enumeracao { get; set; }

        [JsonPropertyName("placeholder")]
        public string? Placeholder { get; set; }

        [JsonPropertyName("readonly")]
        public bool? Readonly { get; set; }

        [JsonPropertyName("_disabled")]
        public bool? Disabled { get; set; }

        [JsonPropertyName("source")]
        public string? Source { get; set; }
    }

    /// <summary>Definição de coluna em tabela/pesquisa</summary>
    public class ColumnDefinition
    {
        [JsonPropertyName("campo")]
        public string? Campo { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("formatador")]
        public string? Formatador { get; set; }

        [JsonPropertyName("largura")]
        public string? Largura { get; set; }
    }

    /// <summary>Definição de filtro em pesquisa</summary>
    public class FilterDefinition
    {
        [JsonPropertyName("campo")]
        public string? Campo { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("tipo")]
        public string? Tipo { get; set; }

        [JsonPropertyName("operadores")]
        public List<string>? Operadores { get; set; }
    }

    /// <summary>Definição de tabela dentro de uma tela</summary>
    public class TableDefinition
    {
        [JsonPropertyName("extremidade")]
        public string? Extremidade { get; set; }

        [JsonPropertyName("endpoint")]
        public string? Endpoint { get; set; }

        [JsonPropertyName("colunas")]
        public List<ColumnDefinition>? Colunas { get; set; }
    }

    /// <summary>Definição de rota</summary>
    public class RouteDefinition
    {
        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("component")]
        public string? Component { get; set; }

        [JsonPropertyName("layout")]
        public string? Layout { get; set; }

        [JsonPropertyName("permission")]
        public string? Permission { get; set; }
    }

    /// <summary>Definição de componente</summary>
    public class ComponentDefinition
    {
        [JsonPropertyName("nome")]
        public string? Nome { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("descricao")]
        public string? Descricao { get; set; }

        [JsonPropertyName("props")]
        public Dictionary<string, object>? Props { get; set; }
    }
}
