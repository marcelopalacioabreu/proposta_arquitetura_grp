using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class AddMunicipioCodigoIbge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop index using migrationBuilder (provider-agnostic)
            migrationBuilder.DropIndex(
                name: "IX_PerfilPermissoes_PerfilId",
                table: "PerfilPermissoes");

            // Add columns to Usuarios using migrationBuilder (provider-agnostic)
            if (migrationBuilder.ActiveProvider?.Contains("Npgsql") == true)
            {
                migrationBuilder.Sql("ALTER TABLE IF EXISTS \"Usuarios\" ADD COLUMN IF NOT EXISTS \"UltimoAcessoOrganizacaoId\" bigint NULL;");
                migrationBuilder.Sql("ALTER TABLE IF EXISTS \"Usuarios\" ADD COLUMN IF NOT EXISTS \"UltimoAcessoOrganizacaoUnidadeId\" bigint NULL;");
                migrationBuilder.Sql("ALTER TABLE IF EXISTS \"Usuarios\" ADD COLUMN IF NOT EXISTS \"UltimoAcessoSetorId\" bigint NULL;");
            }
            else
            {
                migrationBuilder.Sql("ALTER TABLE IF EXISTS Usuarios ADD COLUMN IF NOT EXISTS UltimoAcessoOrganizacaoId bigint NULL;");
                migrationBuilder.Sql("ALTER TABLE IF EXISTS Usuarios ADD COLUMN IF NOT EXISTS UltimoAcessoOrganizacaoUnidadeId bigint NULL;");
                migrationBuilder.Sql("ALTER TABLE IF EXISTS Usuarios ADD COLUMN IF NOT EXISTS UltimoAcessoSetorId bigint NULL;");
            }

            migrationBuilder.AlterColumn<bool>(
                name: "Padrao",
                table: "SetorUsuarios",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            if (migrationBuilder.ActiveProvider?.Contains("Npgsql") == true)
            {
                migrationBuilder.Sql("UPDATE \"OrganizacaoSetores\" SET \"Hierarquia\" = '' WHERE \"Hierarquia\" IS NULL;");
            }
            else
            {
                migrationBuilder.Sql("UPDATE OrganizacaoSetores SET Hierarquia = '' WHERE Hierarquia IS NULL;");
            }

            migrationBuilder.AlterColumn<string>(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodigoIbge",
                table: "Municipios",
                type: "text",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UltimoAcessoOrganizacaoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimoAcessoOrganizacaoUnidadeId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UltimoAcessoSetorId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "CodigoIbge",
                table: "Municipios");

            migrationBuilder.AlterColumn<bool>(
                name: "Padrao",
                table: "SetorUsuarios",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoOrganizacaoId",
                table: "Bairros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoOrganizacaoUnidadeId",
                table: "Bairros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoSetorId",
                table: "Bairros",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId",
                table: "PerfilPermissoes",
                column: "PerfilId");
        }
    }
}




