using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Retaguarda.Persistencia.TracosPadrao
{
    /// <summary>
    /// Traço V1: Campos base de MultilocatarioEntidade
    /// 
    /// Agnóstico de SGBD - suporta PostgreSQL, MySQL, MongoDB
    /// 
    /// Adiciona campos obrigatórios para toda entidade multi-tenant:
    /// - IdentificadorUnico (UUID único por organização)
    /// - IdentificadorUnicoAmigavel (para URLs amigáveis)
    /// - DataInsercao (auditoria)
    /// - DataAlteracao (auditoria)
    /// - OrganizacaoId (multi-tenant)
    /// - OrganizacaoUnidadeId (multi-tenant)
    /// - SetorId (multi-tenant)
    /// - Ativo (soft delete)
    /// - UsuarioInsercaoId (auditoria)
    /// - UsuarioAlteracaoId (auditoria)
    /// - Versao (concorrência otimista)
    /// 
    /// USO:
    /// var tracos = new MultilocatarioEntidadeTracosV1();
    /// tracos.AplicarColunas(migrationBuilder, "MinhaTabela");
    /// </summary>
    public class MultilocatarioEntidadeTracosV1 : ITracoMigracao
    {
        public string Nome => "MultilocatarioEntidadeV1";
        public int Versao => 1;
        public string Descricao => "Campos base de MultilocatarioEntidade (11 campos) - Agnóstico de SGBD";

        /// <summary>
        /// Define um campo do traço - agnóstico de tipo de banco
        /// </summary>
        private class CampoTraco
        {
            public string Nome { get; set; }
            public string TipoClr { get; set; }  // C# type
            public string TipoPostgreSQL { get; set; }
            public string TipoMySQL { get; set; }
            public object ValorPadraoPostgreSQL { get; set; }
            public object ValorPadraoMySQL { get; set; }
            public bool Obrigatorio { get; set; }

            public CampoTraco(string nome, string tipoClr, string postgres, string mysql, 
                            object defaultPostgres = null, object defaultMysql = null, bool obrigatorio = true)
            {
                Nome = nome;
                TipoClr = tipoClr;
                TipoPostgreSQL = postgres;
                TipoMySQL = mysql;
                ValorPadraoPostgreSQL = defaultPostgres;
                ValorPadraoMySQL = defaultMysql;
                Obrigatorio = obrigatorio;
            }
        }

        /// <summary>
        /// Retorna lista de campos agnóstica - dados estruturados
        /// </summary>
        private List<CampoTraco> ObterCampos()
        {
            return new List<CampoTraco>
            {
                new CampoTraco(
                    "IdentificadorUnico", "Guid", 
                    "uuid", "CHAR(36)",
                    "'00000000-0000-0000-0000-000000000000'", "'00000000-0000-0000-0000-000000000000'"),

                new CampoTraco(
                    "IdentificadorUnicoAmigavel", "string",
                    "text", "VARCHAR(255)",
                    "''", "''"),

                new CampoTraco(
                    "DataInsercao", "DateTime",
                    "timestamp without time zone", "DATETIME",
                    "now()", "CURRENT_TIMESTAMP"),

                new CampoTraco(
                    "DataAlteracao", "DateTime?",
                    "timestamp without time zone", "DATETIME",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "OrganizacaoId", "long?",
                    "bigint", "BIGINT",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "OrganizacaoUnidadeId", "long?",
                    "bigint", "BIGINT",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "SetorId", "long?",
                    "bigint", "BIGINT",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "Ativo", "bool",
                    "boolean", "BOOLEAN",
                    "true", "true"),

                new CampoTraco(
                    "UsuarioInsercaoId", "long?",
                    "bigint", "BIGINT",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "UsuarioAlteracaoId", "long?",
                    "bigint", "BIGINT",
                    "NULL", "NULL", obrigatorio: false),

                new CampoTraco(
                    "Versao", "long",
                    "bigint", "BIGINT",
                    "1", "1")
            };
        }

        /// <summary>
        /// Aplica todos os campos do traço a uma tabela existente usando ALTER TABLE.
        /// Automático: Detecta PostgreSQL por padrão, use AplicarColumnasPostgreSQL/MySQL explicitamente para MySQL.
        /// </summary>
        public void AplicarColunas(MigrationBuilder migrationBuilder, string nomeDaTabela)
        {
            // Por padrão, assume PostgreSQL (pode ser melhorado com detecção de provider)
            AplicarColumnasPostgreSQL(migrationBuilder, nomeDaTabela);
        }

        /// <summary>
        /// Aplica campos em POSTGRESQL usando ALTER TABLE com IF NOT EXISTS
        /// </summary>
        public void AplicarColumnasPostgreSQL(MigrationBuilder migrationBuilder, string nomeDaTabela)
        {
            var campos = ObterCampos();
            var sqlParts = new List<string>();

            foreach (var campo in campos)
            {
                sqlParts.Add($@"
    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='{nomeDaTabela}' AND column_name='{campo.Nome}')
    THEN ALTER TABLE ""{nomeDaTabela}"" ADD COLUMN ""{campo.Nome}"" {campo.TipoPostgreSQL} {(campo.Obrigatorio ? "NOT NULL" : "NULL")} DEFAULT {campo.ValorPadraoPostgreSQL};
    END IF;");
            }

            var sqlCompleto = $@"
DO $$
BEGIN
{string.Join("\n", sqlParts)}
END $$;
";
            migrationBuilder.Sql(sqlCompleto);
        }

        /// <summary>
        /// Aplica campos em MYSQL usando ALTER TABLE com IF NOT EXISTS
        /// </summary>
        public void AplicarColumnasMySQL(MigrationBuilder migrationBuilder, string nomeDaTabela)
        {
            var campos = ObterCampos();

            foreach (var campo in campos)
            {
                var sql = $@"
SELECT IF(COUNT(*) = 0,
  'ALTER TABLE `{nomeDaTabela}` ADD COLUMN `{campo.Nome}` {campo.TipoMySQL} {(campo.Obrigatorio ? "NOT NULL" : "NULL")} DEFAULT {campo.ValorPadraoMySQL}',
  'SELECT 1')
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME='{nomeDaTabela}' AND COLUMN_NAME='{campo.Nome}'
";
                migrationBuilder.Sql(sql);
            }
        }

        /// <summary>
        /// Aplica campos em MONGODB (NoSQL - estrutura documental, sem schema)
        /// Nota: MongoDB não requer schema, mas você pode usar isto para validação/índices
        /// </summary>
        public void AplicarColumnasMongoDb(MigrationBuilder migrationBuilder, string nomeColecao)
        {
            // MongoDB não tem schema rígido, mas podemos adicionar validação ou índices
            var campos = ObterCampos();
            var indicesSql = "";

            // Exemplo: Criar índices únicos
            foreach (var campo in campos)
            {
                if (campo.Nome == "IdentificadorUnico")
                {
                    indicesSql += $@"
db.{nomeColecao}.createIndex({{ ""{campo.Nome}"": 1 }}, {{ unique: true }});
";
                }
            }

            if (!string.IsNullOrEmpty(indicesSql))
            {
                migrationBuilder.Sql($"// MongoDB índices para {nomeColecao}\n{indicesSql}");
            }
        }

        /// <summary>
        /// Retorna as definições de coluna em formato C# para usar em CreateTable.
        /// Agnóstico - funciona para qualquer SGBD via EF Core.
        /// </summary>
        public string ObterColunasEmCSharp()
        {
            var campos = ObterCampos();
            var linhas = new List<string>();

            foreach (var campo in campos)
            {
                var tipo = campo.TipoClr switch
                {
                    "Guid" => $"table.Column<Guid>(\"{campo.Nome}\", nullable: false)",
                    "string" => $"table.Column<string>(\"{campo.Nome}\", nullable: false)",
                    "DateTime" => $"table.Column<DateTime>(\"{campo.Nome}\", nullable: false, defaultValueSql: \"now()\")",
                    "DateTime?" => $"table.Column<DateTime?>(\"{campo.Nome}\", nullable: true)",
                    "long?" => $"table.Column<long?>(\"{campo.Nome}\", nullable: true)",
                    "bool" => $"table.Column<bool>(\"{campo.Nome}\", nullable: false, defaultValue: true)",
                    "long" => $"table.Column<long>(\"{campo.Nome}\", nullable: false, defaultValue: 1L)",
                    _ => $"table.Column<object>(\"{campo.Nome}\")"
                };

                linhas.Add(campo.Nome + " = " + tipo);
            }

            return string.Join(",\n                        ", linhas);
        }

        /// <summary>
        /// Retorna SQL PostgreSQL que adiciona TODOS os campos com IF NOT EXISTS.
        /// Iteração dinâmica sobre todas as tabelas.
        /// </summary>
        public string ObterSQLBrutoPorTabelaPostgreSQL()
        {
            var campos = ObterCampos();
            var sqlParts = new List<string>();

            foreach (var campo in campos)
            {
                sqlParts.Add($@"
        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = v_table_name AND column_name = '{campo.Nome}')
        THEN EXECUTE 'ALTER TABLE ' || quote_ident(v_table_name) || ' ADD COLUMN ""{campo.Nome}"" {campo.TipoPostgreSQL} {(campo.Obrigatorio ? "NOT NULL" : "NULL")} DEFAULT {campo.ValorPadraoPostgreSQL}';
        END IF;");
            }

            return $@"
DO $$
DECLARE
    v_table_name TEXT;
BEGIN
    FOR v_table_name IN 
        SELECT table_name 
        FROM information_schema.tables 
        WHERE table_schema = 'public' 
        AND table_name NOT LIKE 'pg_%'
        AND table_name NOT LIKE '__EF%'
        AND table_name != 'Organizacoes'
    LOOP
{string.Join("\n", sqlParts)}
    END LOOP;
END $$;
";
        }

        /// <summary>
        /// Retorna SQL MySQL que adiciona TODOS os campos com IF NOT EXISTS.
        /// </summary>
        public string ObterSQLBrutoPorTabelaMySQL()
        {
            var campos = ObterCampos();
            var sqlStatements = new List<string>();

            // MySQL não tem iteração dinâmica como PostgreSQL, então precisamos listar as tabelas
            // Esta é uma abordagem usando múltiplos ALTERs
            sqlStatements.Add(@"
SELECT @tabelas := GROUP_CONCAT(TABLE_NAME) 
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_SCHEMA = DATABASE()
");

            // Para cada tabela, adicionar os campos
            var colunasSQL = "";
            foreach (var campo in campos)
            {
                colunasSQL += $@"
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'SuaTabela' AND COLUMN_NAME = '{campo.Nome}'
)
THEN
    ALTER TABLE SuaTabela ADD COLUMN {campo.Nome} {campo.TipoMySQL} {(campo.Obrigatorio ? "NOT NULL" : "NULL")} DEFAULT {campo.ValorPadraoMySQL};
END IF;
";
            }

            return $@"
-- MySQL: Adicionar campos traço V1 a cada tabela
-- NOTA: Execute uma vez por tabela, substituindo 'SuaTabela' pelo nome real
{colunasSQL}
";
        }

        /// <summary>
        /// Retorna SQL PostgreSQL que adiciona TODOS os campos com IF NOT EXISTS.
        /// Compatibilidade com código antigo.
        /// </summary>
        public string ObterSQLBrutoPorTabela()
        {
            return ObterSQLBrutoPorTabelaPostgreSQL();
        }

        /// <summary>
        /// Retorna informações sobre os campos em formato agnóstico (para documentação, geração de código, etc.)
        /// </summary>
        public IReadOnlyList<(string Nome, string TipoClr, string TipoPostgreSQL, string TipoMySQL)> ObterInfoCampos()
        {
            var campos = ObterCampos();
            return campos.Select(c => (c.Nome, c.TipoClr, c.TipoPostgreSQL, c.TipoMySQL)).ToList().AsReadOnly();
        }

        /// <summary>
        /// Exemplo de uso para MongoDB - retorna documentação JSON Schema
        /// </summary>
        public string ObterEsquemaMongoDBJson()
        {
            var campos = ObterCampos();
            var props = new List<string>();

            foreach (var campo in campos)
            {
                var tipo = campo.TipoClr switch
                {
                    "Guid" => "\"type\": \"string\", \"format\": \"uuid\"",
                    "string" => "\"type\": \"string\"",
                    "DateTime" => "\"type\": \"string\", \"format\": \"date-time\"",
                    "DateTime?" => "\"type\": [\"string\", \"null\"], \"format\": \"date-time\"",
                    "long?" => "\"type\": [\"integer\", \"null\"]",
                    "bool" => "\"type\": \"boolean\"",
                    "long" => "\"type\": \"integer\"",
                    _ => "\"type\": \"object\""
                };

                props.Add($"    \"{campo.Nome}\": {{ {tipo} }}");
            }

            return $@"
{{
  ""$schema"": ""http://json-schema.org/draft-07/schema#"",
  ""title"": ""MultilocatarioEntidadeV1"",
  ""type"": ""object"",
  ""properties"": {{
{string.Join(",\n", props)}
  }}
}}";
        }
    }
}
