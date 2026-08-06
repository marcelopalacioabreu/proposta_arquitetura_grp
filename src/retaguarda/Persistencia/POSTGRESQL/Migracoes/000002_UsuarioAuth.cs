using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
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
                    type: "text",
                nullable: true );

            migrationBuilder.AddColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                    type: "text",
                nullable: false );

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                    type: "text",
                nullable: false );

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




