using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class UpdateSituacaoAndTipoModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentoTipos");

            migrationBuilder.DropTable(
                name: "SituacaoContextos");

            migrationBuilder.DropTable(
                name: "SituacaoImovel");

            migrationBuilder.DropTable(
                name: "TipoContatos");

            migrationBuilder.DropTable(
                name: "TipoContextos");

            migrationBuilder.DropTable(
                name: "TipoEnderecos");

            migrationBuilder.DropTable(
                name: "TipoImovel");

            migrationBuilder.DropTable(
                name: "TipoUnidade");

            migrationBuilder.AddColumn<string>(
                name: "Contexto",
                table: "Tipos",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Tipos",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "Tipos",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Contexto",
                table: "Situacoes",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "idx_Tipos_Codigo_Contexto_Unico",
                table: "Tipos",
                columns: new[] { "Codigo", "Contexto", "OrganizacaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_Tipos_Contexto_Ativo",
                table: "Tipos",
                columns: new[] { "OrganizacaoId", "Contexto", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Codigo_Contexto_Unico",
                table: "Situacoes",
                columns: new[] { "Codigo", "Contexto", "OrganizacaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Contexto_Ativo",
                table: "Situacoes",
                columns: new[] { "OrganizacaoId", "Contexto", "Ativo" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_Tipos_Codigo_Contexto_Unico",
                table: "Tipos");

            migrationBuilder.DropIndex(
                name: "idx_Tipos_Contexto_Ativo",
                table: "Tipos");

            migrationBuilder.DropIndex(
                name: "idx_Situacoes_Codigo_Contexto_Unico",
                table: "Situacoes");

            migrationBuilder.DropIndex(
                name: "idx_Situacoes_Contexto_Ativo",
                table: "Situacoes");

            migrationBuilder.DropColumn(
                name: "Contexto",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Contexto",
                table: "Situacoes");

            migrationBuilder.CreateTable(
                name: "DocumentoTipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoTipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SituacaoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacaoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SituacaoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacaoImovel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoContatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEnderecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoImovel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoUnidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUnidade", x => x.Id);
                });
        }
    }
}
