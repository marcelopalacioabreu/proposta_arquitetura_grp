using Microsoft.EntityFrameworkCore.Migrations;

namespace Retaguarda.Persistencia.TracosPadrao
{
    /// <summary>
    /// Interface padrão para traços de migração reutilizáveis.
    /// Cada versão de traço define um conjunto de colunas que devem ser aplicadas a todas as tabelas.
    /// </summary>
    public interface ITracoMigracao
    {
        /// <summary>
        /// Nome do traço (ex: MultilocatarioEntidadeV1, MultilocatarioEntidadeV2)
        /// </summary>
        string Nome { get; }

        /// <summary>
        /// Versão do traço (incrementar quando adicionar novos campos)
        /// </summary>
        int Versao { get; }

        /// <summary>
        /// Descrição dos campos que este traço adiciona
        /// </summary>
        string Descricao { get; }

        /// <summary>
        /// Aplica as colunas do traço a uma tabela específica.
        /// Deve usar IF NOT EXISTS para ser idempotent.
        /// </summary>
        void AplicarColunas(MigrationBuilder migrationBuilder, string nomeDaTabela);

        /// <summary>
        /// SQL bruto para aplicar o traço em TODAS as tabelas.
        /// Executado como um bloco DO de uma vez.
        /// </summary>
        string ObterSQLBrutoPorTabela();
    }
}
