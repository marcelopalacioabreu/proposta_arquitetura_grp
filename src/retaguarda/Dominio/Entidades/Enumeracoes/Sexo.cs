using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Representa os diferentes sexos de uma pessoa.
    /// </summary>
    public class Sexo : IEnumeracao
    {
        public string Chave { get; set; }
        public string Descricao { get; set; }

        private Sexo(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        // Constantes de contextos
        public const string MASCULINO = "MASCULINO";
        public const string FEMININO = "FEMININO";

        // Instâncias dos valores
        public static readonly Sexo Masculino = new(MASCULINO, "Masculino");
        public static readonly Sexo Feminino = new(FEMININO, "Feminino");

        /// <summary>
        /// Retorna todos os contextos disponíveis.
        /// </summary>
        public static IEnumerable<Sexo> Todos => new[]
        {
            Masculino, Feminino
        };

        /// <summary>
        /// Obtém o contexto pela chave.
        /// </summary>
        public static Sexo ObterPorChave(string chave)
        {
            return chave switch
            {
                MASCULINO => Masculino,
                FEMININO => Feminino,
                _ => Masculino // Valor padrão
            };
        }

        /// <summary>
        /// Obtém a descrição a partir da chave.
        /// </summary>
        public static string ObterDescricao(string chave)
        {
            return ObterPorChave(chave).Descricao;
        }
    }
}
