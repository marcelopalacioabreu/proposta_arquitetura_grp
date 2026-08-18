using System;
using System.Collections.Generic;
using System.Linq;

namespace Retaguarda.Dominio.Entidades.Enumeracoes
{
    /// <summary>
    /// Helper estático com métodos comuns para trabalhar com enumerações.
    /// Oferece conversão entre chave e descrição.
    /// </summary>
    public static class EnumeracaoHelper
    {
        /// <summary>
        /// Converte uma chave em sua descrição correspondente.
        /// </summary>
        public static string ObterDescricao<T>(string chave, IEnumerable<T> valores) where T : IEnumeracao
        {
            var item = valores.FirstOrDefault(x => x.Chave == chave);
            return item?.Descricao ?? chave;
        }

        /// <summary>
        /// Converte uma descrição em sua chave correspondente.
        /// </summary>
        public static string ObterChave<T>(string descricao, IEnumerable<T> valores) where T : IEnumeracao
        {
            var item = valores.FirstOrDefault(x => x.Descricao == descricao);
            return item?.Chave ?? string.Empty;
        }

        /// <summary>
        /// Retorna todos os pares chave-descrição para seleções em telas.
        /// </summary>
        public static List<(string chave, string descricao)> ObterTodos<T>(IEnumerable<T> valores) where T : IEnumeracao
        {
            return valores.Select(x => (x.Chave, x.Descricao)).ToList();
        }

        /// <summary>
        /// Valida se uma chave existe na enumeração.
        /// </summary>
        public static bool ValidarChave<T>(string chave, IEnumerable<T> valores) where T : IEnumeracao
        {
            return valores.Any(x => x.Chave == chave);
        }
    }
}
