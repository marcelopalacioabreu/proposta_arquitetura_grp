using System;
using System.Collections.Generic;

namespace Retaguarda.Metadados.Contracts
{
    /// <summary>Interface para enumerações com conversão entre valor (BD) e texto (UI)</summary>
    public interface IEnumeracao
    {
        /// <summary>Valor armazenado no banco de dados</summary>
        string Valor { get; }
        
        /// <summary>Texto exibido na interface do usuário</summary>
        string Texto { get; }
        
        /// <summary>Descrição opcional para tooltips e ajuda</summary>
        string? Descricao { get; }
    }

    /// <summary>
    /// Classe base abstrata para enumerações com valores conhecidos
    /// 
    /// Exemplo de uso:
    /// 
    /// public class TipoPessoa : Enumeracao
    /// {
    ///     public static readonly TipoPessoa Fisica = new("F", "Pessoa Física");
    ///     public static readonly TipoPessoa Juridica = new("J", "Pessoa Jurídica");
    ///     
    ///     private TipoPessoa(string valor, string texto, string? descricao = null)
    ///         : base(valor, texto, descricao) { }
    ///     
    ///     public static TipoPessoa? ConverterDe(string? valor) =>
    ///         valor?.ToUpper() switch {
    ///             "F" => Fisica,
    ///             "J" => Juridica,
    ///             _ => null
    ///         };
    /// }
    /// </summary>
    public abstract class Enumeracao : IEnumeracao, IEquatable<Enumeracao>
    {
        /// <summary>Valor armazenado no banco de dados</summary>
        public string Valor { get; }

        /// <summary>Texto exibido na UI</summary>
        public string Texto { get; }

        /// <summary>Descrição opcional</summary>
        public string? Descricao { get; }

        /// <summary>Inicializa uma nova instância de enumeração</summary>
        /// <param name="valor">Valor a ser armazenado no banco (não pode ser nulo ou vazio)</param>
        /// <param name="texto">Texto a ser exibido na UI (não pode ser nulo ou vazio)</param>
        /// <param name="descricao">Descrição opcional para tooltips</param>
        protected Enumeracao(string valor, string texto, string? descricao = null)
        {
            Valor = valor ?? throw new ArgumentNullException(nameof(valor));
            if (string.IsNullOrWhiteSpace(valor))
                throw new ArgumentException("Valor não pode ser vazio", nameof(valor));
            
            Texto = texto ?? throw new ArgumentNullException(nameof(texto));
            if (string.IsNullOrWhiteSpace(texto))
                throw new ArgumentException("Texto não pode ser vazio", nameof(texto));
            
            Descricao = descricao;
        }

        /// <summary>Retorna o texto da enumeração</summary>
        public override string ToString() => Texto;

        /// <summary>Compara com outra enumeração pelo valor</summary>
        public override bool Equals(object? obj) =>
            obj is Enumeracao e && e.Valor == Valor;

        /// <summary>Retorna hash code baseado no valor</summary>
        public override int GetHashCode() => Valor.GetHashCode();

        /// <summary>Compara com outra enumeração pelo valor</summary>
        public bool Equals(Enumeracao? other) =>
            other is not null && other.Valor == Valor;

        /// <summary>Implementa operador de igualdade</summary>
        public static bool operator ==(Enumeracao? left, Enumeracao? right) =>
            Equals(left, right);

        /// <summary>Implementa operador de desigualdade</summary>
        public static bool operator !=(Enumeracao? left, Enumeracao? right) =>
            !Equals(left, right);
    }

    /// <summary>Item de enumeração com valor e texto (para JSON e API)</summary>
    public class ItemEnumeracao
    {
        public string Valor { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }

    /// <summary>Definição completa de uma enumeração (carregada do JSON)</summary>
    public class DefinicaoEnumeracao
    {
        public string Chave { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<ItemEnumeracao> Valores { get; set; } = new();
        public List<GrupoEnumeracao>? Grupos { get; set; }
    }

    /// <summary>Grupo opcional para organizar itens de enumeração</summary>
    public class GrupoEnumeracao
    {
        public string Nome { get; set; } = string.Empty;
        public List<ItemEnumeracao> Valores { get; set; } = new();
    }
}
