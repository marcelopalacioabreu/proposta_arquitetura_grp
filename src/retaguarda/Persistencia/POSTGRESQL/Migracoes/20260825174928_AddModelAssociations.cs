using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class AddModelAssociations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PessoaId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsavelId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsavelSetorId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CepId",
                table: "Municipios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BairroId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "LogradouroId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MunicipioId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PaisId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UfId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "ResponsavelId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "ResponsavelSetorId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "CepId",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "BairroId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "LogradouroId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "MunicipioId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "PaisId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "UfId",
                table: "Enderecos");
        }
    }
}
