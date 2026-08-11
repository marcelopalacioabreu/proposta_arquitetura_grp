using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    [Migration("000009_AddOrquestracaoFluxoProcesso")]
    public partial class AddOrquestracaoFluxoProcesso : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrquestracaoFluxoProcessos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: true),
                    WorkflowDefinitionId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    WorkflowVersion = table.Column<int>(type: "integer", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrquestracaoFluxoProcessos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrquestracaoFluxoProcessos");
        }
    }
}
