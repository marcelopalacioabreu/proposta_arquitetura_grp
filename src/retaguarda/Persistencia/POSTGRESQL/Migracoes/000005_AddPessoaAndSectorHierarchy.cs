using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    [DbContext(typeof(Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext))]
    [Migration("20260804120000_AddPessoaAndSectorHierarchy")]
    public partial class AddPessoaAndSectorHierarchy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true );

            migrationBuilder.AddColumn<long>(
                name: "SetorPaiId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Padrao",
                table: "SetorUsuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "PessoaId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        ,
                    TipoPessoaChave = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        ,
                    Documento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        ,
                    Telefone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        ,
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                } );

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId",
                principalTable: "OrganizacaoSetores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Pessoas_PessoaId",
                table: "Usuarios",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Pessoas_PessoaId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropIndex(
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Hierarquia",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "Padrao",
                table: "SetorUsuarios");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Usuarios");
        }
    }
}




