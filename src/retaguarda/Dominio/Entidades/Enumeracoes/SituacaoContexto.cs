using System;
using System.Collections.Generic;
using System.Linq;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    public class SituacaoContexto : IEnumeracao
    {
        // Constantes de contexto
        public const string IMOVEL = "IMOVEL";

        // Instâncias estáticas
        public static readonly SituacaoContexto Imovel = new(IMOVEL, "Imóvel");

        // Propriedades
        public string Chave { get; }
        public string Descricao { get; }

        private SituacaoContexto(string chave, string descricao)
        {
            Chave = chave;
            Descricao = descricao;
        }

        /// <summary>
        /// Obtém a descrição de um contexto pela chave.
        /// </summary>
        public static string ObterDescricao(string chave)
        {
            return chave switch
            {
                IMOVEL => "Imóvel",
                _ => chave
            };
        }

        /// <summary>
        /// Obtém uma instância de SituacaoContexto pela chave.
        /// </summary>
        public static SituacaoContexto ObterPorChave(string chave)
        {
            return chave switch
            {
                IMOVEL => Imovel,
                _ => throw new ArgumentException($"Contexto de situação inválido: {chave}")
            };
        }

        /// <summary>
        /// Retorna todos os contextos disponíveis.
        /// </summary>
        public static IEnumerable<SituacaoContexto> Todos =>
            new[]
            {
                Imovel
            };

        public override string ToString() => Descricao;
        public override bool Equals(object obj) => obj is SituacaoContexto ec && Chave == ec.Chave;
        public override int GetHashCode() => Chave.GetHashCode();
    }
}
