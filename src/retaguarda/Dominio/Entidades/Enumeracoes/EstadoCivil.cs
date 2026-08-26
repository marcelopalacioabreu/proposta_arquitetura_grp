using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Representa os diferentes estados civis de uma pessoa.
    /// </summary>
    public class EstadoCivil : IEnumeracao
    {
        public string Chave { get; set; }
        public string Descricao { get; set; }

        private EstadoCivil(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        // Constantes de contextos
        public const string SOLTEIRO = "SOLTEIRO";
        public const string CASADO = "CASADO";
        public const string DIVORCIADO = "DIVORCIADO";
        public const string VIUVO = "VIUVO";
        public const string UNIAO_ESTAVEL = "UNIAO_ESTAVEL";

        // Instâncias dos valores
        public static readonly EstadoCivil Solteiro = new(SOLTEIRO, "Solteiro");
        public static readonly EstadoCivil Casado = new(CASADO, "Casado");
        public static readonly EstadoCivil Divorciado = new(DIVORCIADO, "Divorciado");
        public static readonly EstadoCivil Viuvo = new(VIUVO, "Viúvo");
        public static readonly EstadoCivil UniaoEstavel = new(UNIAO_ESTAVEL, "União Estável");

        /// <summary>
        /// Retorna todos os contextos disponíveis.
        /// </summary>
        public static IEnumerable<EstadoCivil> Todos => new[]
        {
            Solteiro, Casado, Divorciado, Viuvo, UniaoEstavel
        };

        /// <summary>
        /// Obtém o contexto pela chave.
        /// </summary>
        public static EstadoCivil ObterPorChave(string chave)
        {
            return chave switch
            {
                SOLTEIRO => Solteiro,
                CASADO => Casado,
                DIVORCIADO => Divorciado,
                VIUVO => Viuvo,
                UNIAO_ESTAVEL => UniaoEstavel,
                _ => Solteiro // Valor padrão
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
