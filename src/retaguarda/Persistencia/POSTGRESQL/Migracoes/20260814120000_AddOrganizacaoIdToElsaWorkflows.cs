using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class AddOrganizacaoIdToElsaWorkflows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CORREÇÃO 3: Adicionar coluna OrganizacaoId ao schema Elsa
            // Necessário para filtrar workflows por tenant (organização)
            
            // Adicionar coluna a WorkflowDefinitions
            if (!migrationBuilder.ActiveProvider.Contains("Sqlite"))
            {
                migrationBuilder.AddColumn<long>(
                    name: "OrganizacaoId",
                    table: "WorkflowDefinitions",
                    type: "bigint",
                    nullable: true,
                    comment: "ID da organização (tenant) que possui este workflow");
            }
            else
            {
                migrationBuilder.AddColumn<long>(
                    name: "OrganizacaoId",
                    table: "WorkflowDefinitions",
                    type: "INTEGER",
                    nullable: true);
            }

            // Criar índice para melhorar performance de queries filtradas por OrganizacaoId
            migrationBuilder.CreateIndex(
                name: "IX_WorkflowDefinitions_OrganizacaoId",
                table: "WorkflowDefinitions",
                column: "OrganizacaoId",
                filter: "\"OrganizacaoId\" IS NOT NULL");

            // Adicionar coluna a WorkflowInstances (instâncias de execução)
            if (!migrationBuilder.ActiveProvider.Contains("Sqlite"))
            {
                migrationBuilder.AddColumn<long>(
                    name: "OrganizacaoId",
                    table: "WorkflowInstances",
                    type: "bigint",
                    nullable: true,
                    comment: "ID da organização (tenant) que executa este workflow");
            }
            else
            {
                migrationBuilder.AddColumn<long>(
                    name: "OrganizacaoId",
                    table: "WorkflowInstances",
                    type: "INTEGER",
                    nullable: true);
            }

            // Criar índice para melhorar performance de queries filtradas por OrganizacaoId
            migrationBuilder.CreateIndex(
                name: "IX_WorkflowInstances_OrganizacaoId",
                table: "WorkflowInstances",
                column: "OrganizacaoId",
                filter: "\"OrganizacaoId\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover índices
            migrationBuilder.DropIndex(
                name: "IX_WorkflowDefinitions_OrganizacaoId",
                table: "WorkflowDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_WorkflowInstances_OrganizacaoId",
                table: "WorkflowInstances");

            // Remover colunas
            migrationBuilder.DropColumn(
                name: "OrganizacaoId",
                table: "WorkflowDefinitions");

            migrationBuilder.DropColumn(
                name: "OrganizacaoId",
                table: "WorkflowInstances");
        }
    }
}
