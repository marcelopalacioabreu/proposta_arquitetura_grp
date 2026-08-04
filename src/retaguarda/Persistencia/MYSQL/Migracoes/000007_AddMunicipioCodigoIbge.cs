using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.MYSQL.Migracoes
{
    /// <inheritdoc />
    public partial class AddMunicipioCodigoIbge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS `IX_PerfilPermissoes_PerfilId` ON `PerfilPermissoes`;");

            // Removidas tentativas de remover colunas de Bairros por serem instáveis em esquemas limpos.

            migrationBuilder.Sql("ALTER TABLE `Usuarios` ADD COLUMN IF NOT EXISTS `UltimoAcessoOrganizacaoId` bigint NULL;");
            migrationBuilder.Sql("ALTER TABLE `Usuarios` ADD COLUMN IF NOT EXISTS `UltimoAcessoOrganizacaoUnidadeId` bigint NULL;");
            migrationBuilder.Sql("ALTER TABLE `Usuarios` ADD COLUMN IF NOT EXISTS `UltimoAcessoSetorId` bigint NULL;");

            migrationBuilder.AlterColumn<bool>(
                name: "Padrao",
                table: "SetorUsuarios",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)");

            migrationBuilder.Sql("UPDATE OrganizacaoSetores SET Hierarquia = '' WHERE Hierarquia IS NULL;");

            migrationBuilder.AlterColumn<string>(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "CodigoIbge",
                table: "Municipios",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
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
                type: "tinyint(1)",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(1000)",
                oldMaxLength: 1000)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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
