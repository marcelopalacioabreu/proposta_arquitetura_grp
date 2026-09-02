using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Enumeração de Contextos de Organizações.
    /// Define os tipos de classificações disponíveis para Tipos e Situações no contexto de Organizações.
    /// Valores armazenados como chaves no banco de dados na coluna Contexto das tabelas Tipos e Situacoes.
    /// </summary>
    public class OrganizacaoContexto : IEnumeracao
    {
        public string Chave { get; }
        public string Descricao { get; }

        private OrganizacaoContexto(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        // Constantes de contextos
        public const string TIPO = "ORGANIZACAO_TIPO";
        public const string SITUACAO = "ORGANIZACAO_SITUACAO";

        // Instâncias dos valores
        public static readonly OrganizacaoContexto Tipo = new(TIPO, "Tipo de Organização");
        public static readonly OrganizacaoContexto Situacao = new(SITUACAO, "Situação de Organização");

        /// <summary>
        /// Retorna todos os contextos disponíveis.
        /// </summary>
        public static IEnumerable<OrganizacaoContexto> Todos => new[]
        {
            Tipo, Situacao
        };

        /// <summary>
        /// Obtém o contexto pela chave.
        /// </summary>
        public static OrganizacaoContexto ObterPorChave(string chave)
        {
            return chave switch
            {
                TIPO => Tipo,
                SITUACAO => Situacao,
                _ => throw new System.ArgumentException($"Contexto de organização inválido: {chave}")
            };
        }

        /// <summary>
        /// Obtém a descrição de um contexto pela chave.
        /// </summary>
        public static string ObterDescricao(string chave)
        {
            return chave switch
            {
                TIPO => "Tipo de Organização",
                SITUACAO => "Situação de Organização",
                _ => chave
            };
        }
    }
}
