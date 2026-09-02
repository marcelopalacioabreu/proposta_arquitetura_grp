using System.Collections.Generic;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Enumeração de Contextos de Tipo.
    /// Cada contexto agrupa um conjunto de tipos relacionados (Endereço, Contato, Unidade, etc).
    /// Valores armazenados como chaves no banco de dados na coluna Contexto da tabela Tipos.
    /// </summary>
    public class TipoContexto : IEnumeracao
    {
        public string Chave { get; set; }
        public string Descricao { get; set; }

        private TipoContexto(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        // Constantes de contextos
        public const string ENDERECO = "ENDERECO";
        public const string CONTATO = "CONTATO";
        public const string UNIDADE = "UNIDADE";
        public const string IMOVEL = "IMOVEL";
        public const string DOCUMENTO = "DOCUMENTO";
        public const string ORGANIZACAO = "ORGANIZACAO_TIPO";

        // Instâncias dos valores
        public static readonly TipoContexto Endereco = new(ENDERECO, "Tipo de Endereço");
        public static readonly TipoContexto Contato = new(CONTATO, "Tipo de Contato");
        public static readonly TipoContexto Unidade = new(UNIDADE, "Tipo de Unidade");
        public static readonly TipoContexto Imovel = new(IMOVEL, "Tipo de Imóvel");
        public static readonly TipoContexto Documento = new(DOCUMENTO, "Tipo de Documento");
        public static readonly TipoContexto Organizacao = new(ORGANIZACAO, "Tipo de Organização");

        /// <summary>
        /// Retorna todos os contextos disponíveis.
        /// </summary>
        public static IEnumerable<TipoContexto> Todos => new[]
        {
            Endereco, Contato, Unidade, Imovel, Documento, Organizacao
        };

        /// <summary>
        /// Obtém o contexto pela chave.
        /// </summary>
        public static TipoContexto ObterPorChave(string chave)
        {
            return chave switch
            {
                ENDERECO => Endereco,
                CONTATO => Contato,
                UNIDADE => Unidade,
                IMOVEL => Imovel,
                DOCUMENTO => Documento,
                ORGANIZACAO => Organizacao,
                _ => Endereco // Valor padrão
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
