using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Enumeração de Tipos de Pessoa (Física ou Jurídica).
    /// Valores armazenados como chaves no banco: "F" = Física, "J" = Jurídica.
    /// Textos exibidos na tela via Descricao.
    /// </summary>
    public class PessoaTipo : IEnumeracao
    {
        public string Chave { get; set; }
        public string Descricao { get; set; }

        private PessoaTipo(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        // Constantes de tipos
        public const string FISICA = "F";
        public const string JURIDICA = "J";

        // Instâncias dos valores
        public static readonly PessoaTipo Fisica = new(FISICA, "Física");
        public static readonly PessoaTipo Juridica = new(JURIDICA, "Jurídica");

        /// <summary>
        /// Retorna todos os valores disponíveis.
        /// </summary>
        public static IEnumerable<PessoaTipo> Todos => new[] { Fisica, Juridica };

        /// <summary>
        /// Obtém o tipo de pessoa pela chave.
        /// </summary>
        public static PessoaTipo ObterPorChave(string chave)
        {
            return chave switch
            {
                FISICA => Fisica,
                JURIDICA => Juridica,
                _ => Fisica // Valor padrão
            };
        }

        /// <summary>
        /// Obtém a descrição (texto) a partir da chave.
        /// </summary>
        public static string ObterDescricao(string chave)
        {
            return EnumeracaoHelper.ObterDescricao(chave, Todos);
        }
    }
}
