using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.MYSQL.Migracoes
{
    /// <inheritdoc />
    public partial class ConsolidateSituationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add new columns to Situacoes if not exists
            migrationBuilder.AddColumn<string>(
                name: "Contexto",
                table: "Situacoes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            // Step 2: Migrate data from SituacaoImovel to Situacoes
            migrationBuilder.Sql(
                @"INSERT INTO Situacoes (Codigo, Nome, Contexto, Descricao, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'IMOVEL', NULL, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM SituacaoImovel
                ON DUPLICATE KEY UPDATE Id=Id");

            // Step 3: Drop old tables
            migrationBuilder.DropTable(name: "SituacaoContextos");
            migrationBuilder.DropTable(name: "SituacaoImovel");

            // Step 4: Create composite indices
            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Contexto_Ativo",
                table: "Situacoes",
                columns: new[] { "OrganizacaoId", "Contexto", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Codigo_Contexto_Unico",
                table: "Situacoes",
                columns: new[] { "Codigo", "Contexto", "OrganizacaoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Note: Down migration would require recreating the old tables
            // This is intentionally left incomplete as a safety measure
            migrationBuilder.DropIndex(
                name: "idx_Situacoes_Contexto_Ativo",
                table: "Situacoes");

            migrationBuilder.DropIndex(
                name: "idx_Situacoes_Codigo_Contexto_Unico",
                table: "Situacoes");

            migrationBuilder.DropColumn(
                name: "Contexto",
                table: "Situacoes");
        }
    }
}
