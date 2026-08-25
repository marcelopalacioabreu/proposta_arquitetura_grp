using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class AlignModelWithUMLDesign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizacaoSetores_Organizacoes_OrganizacaoId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropForeignKey(
                name: "FK_OrganizacaoUnidades_Organizacoes_OrganizacaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropForeignKey(
                name: "FK_PerfilPermissoes_Organizacoes_OrganizacaoId",
                table: "PerfilPermissoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Perfis_Organizacoes_OrganizacaoId",
                table: "Perfis");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Pessoas_PessoaId",
                table: "Usuarios");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Perfis_OrganizacaoId",
                table: "Perfis");

            migrationBuilder.DropIndex(
                name: "IX_PerfilPermissoes_OrganizacaoId",
                table: "PerfilPermissoes");

            migrationBuilder.DropIndex(
                name: "IX_OrganizacaoUnidades_OrganizacaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropIndex(
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "PessoaId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Documento",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "EstadoCivilId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NacionalidadePaisId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NaturalidadeMunicipioId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "SexoId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "TipoPessoaChave",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "DataExtincao",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "InscricaoEstadual",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "InscricaoMunicipal",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "NaturezaJuridicaId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "NivelGovernoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "NomeFantasia",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "TipoOrganizacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "DataExtincao",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ResponsavelPessoaId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "TipoUnidadeId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "SetorPaiId",
                table: "OrganizacaoSetores");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Pessoas",
                newName: "InscricaoMunicipal");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "PerfilPermissoes",
                newName: "Chave");

            migrationBuilder.RenameIndex(
                name: "IX_PerfilPermissoes_PerfilId_Nome",
                table: "PerfilPermissoes",
                newName: "IX_PerfilPermissoes_PerfilId_Chave");

            migrationBuilder.RenameColumn(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                newName: "CodigoHierarquico");

            migrationBuilder.AlterColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Situacoes",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Pcd",
                table: "Pessoas",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "NomeSocial",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NomePai",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NomeMae",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Pessoas",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Anotacoes",
                table: "Pessoas",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Pessoas",
                type: "character varying(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "Pessoas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Discriminator",
                table: "Pessoas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EstadoCivil",
                table: "Pessoas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Pessoas",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFantasia",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Pessoas",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TipoPessoa",
                table: "Pessoas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "AdministradorDoSistema",
                table: "Perfis",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<long>(
                name: "Nivel",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SituacaoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_SituacaoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_TipoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SituacaoContextos");

            migrationBuilder.DropTable(
                name: "TipoContextos");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Situacoes");

            migrationBuilder.DropColumn(
                name: "Anotacoes",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "DataExtincao",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "EstadoCivil",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "InscricaoEstadual",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeFantasia",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Pessoas");

            migrationBuilder.RenameColumn(
                name: "InscricaoMunicipal",
                table: "Pessoas",
                newName: "Telefone");

            migrationBuilder.RenameColumn(
                name: "Chave",
                table: "PerfilPermissoes",
                newName: "Nome");

            migrationBuilder.RenameIndex(
                name: "IX_PerfilPermissoes_PerfilId_Chave",
                table: "PerfilPermissoes",
                newName: "IX_PerfilPermissoes_PerfilId_Nome");

            migrationBuilder.RenameColumn(
                name: "CodigoHierarquico",
                table: "OrganizacaoSetores",
                newName: "Hierarquia");

            migrationBuilder.AlterColumn<string>(
                name: "SenhaHash",
                table: "Usuarios",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Usuarios",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PessoaId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Pcd",
                table: "Pessoas",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeSocial",
                table: "Pessoas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomePai",
                table: "Pessoas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NomeMae",
                table: "Pessoas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nome",
                table: "Pessoas",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cpf",
                table: "Pessoas",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(14)",
                oldMaxLength: 14,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Pessoas",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Pessoas",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EstadoCivilId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NacionalidadePaisId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NaturalidadeMunicipioId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SexoId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPessoaChave",
                table: "Pessoas",
                type: "character varying(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "AdministradorDoSistema",
                table: "Perfis",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Organizacoes",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "Organizacoes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "Organizacoes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Organizacoes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InscricaoMunicipal",
                table: "Organizacoes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "NaturezaJuridicaId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Nivel",
                table: "Organizacoes",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "NivelGovernoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFantasia",
                table: "Organizacoes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Organizacoes",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TipoOrganizacaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "Nivel",
                table: "OrganizacaoUnidades",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "OrganizacaoUnidades",
                type: "character varying(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "OrganizacaoUnidades",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "OrganizacaoUnidades",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsavelPessoaId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TipoUnidadeId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "OrganizacaoUnidades",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "OrganizacaoUnidades",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SetorPaiId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Funcoes_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_OrganizacaoId",
                table: "Perfis",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_OrganizacaoId",
                table: "PerfilPermissoes",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_OrganizacaoId",
                table: "OrganizacaoUnidades",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcoes_OrganizacaoId",
                table: "Funcoes",
                column: "OrganizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId",
                principalTable: "OrganizacaoSetores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoSetores_Organizacoes_OrganizacaoId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoUnidades_Organizacoes_OrganizacaoId",
                table: "OrganizacaoUnidades",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PerfilPermissoes_Organizacoes_OrganizacaoId",
                table: "PerfilPermissoes",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Organizacoes_OrganizacaoId",
                table: "Perfis",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Pessoas_PessoaId",
                table: "Usuarios",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
