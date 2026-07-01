using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.Migracoes.MYSQL
{
    /// <inheritdoc />
    public partial class _000002_UsuarioAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Usuarios",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Perfis",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "PerfilPermissoes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "OrganizacaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrganizacaoSetorId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Funcoes",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SenhaHash",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "OrganizacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "OrganizacaoSetorId",
                table: "Organizacoes");

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Usuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Perfis",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "PerfilPermissoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "OrganizacaoId",
                table: "Funcoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
