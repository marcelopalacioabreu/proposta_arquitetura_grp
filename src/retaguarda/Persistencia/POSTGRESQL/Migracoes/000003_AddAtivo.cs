using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class AddAtivo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Usuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Perfis",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "PerfilPermissoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Organizacoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "OrganizacaoSetores",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Ativo",
                table: "Funcoes",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "Ativo",
                table: "Usuarios");
        }
    }
}




