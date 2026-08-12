using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.MYSQL.Migracoes
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ContatoRelacionamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ContatoId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoRelacionamentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TipoContatoId = table.Column<long>(type: "bigint", nullable: true),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ContatoValor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentoRelacionamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentoId = table.Column<long>(type: "bigint", nullable: false),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoRelacionamentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DocumentoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    Numero = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Digito = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OrgaoEmissor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UfEmissor = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataEmissao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataValidade = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Principal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Validado = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DocumentoTipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoTipos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NaturezasJuridicas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NaturezasJuridicas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NiveisGoverno",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NiveisGoverno", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoSetorEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoSetorEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidadeEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoUnidadeEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Organizacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sigla = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RazaoSocial = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeFantasia = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InscricaoEstadual = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InscricaoMunicipal = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoOrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    NivelGovernoId = table.Column<long>(type: "bigint", nullable: true),
                    NaturezaJuridicaId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoPaiId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoRaizId = table.Column<long>(type: "bigint", nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
                    DataFundacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataExtincao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HierarquiaCodigo = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<short>(type: "smallint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizacoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrquestracaoFluxoProcessos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Descricao = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkflowDefinitionId = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WorkflowVersion = table.Column<int>(type: "int", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrquestracaoFluxoProcessos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Paises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paises", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PessoaEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PessoaId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PessoaEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomeSocial = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cpf = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    SexoId = table.Column<long>(type: "bigint", nullable: true),
                    EstadoCivilId = table.Column<long>(type: "bigint", nullable: true),
                    NacionalidadePaisId = table.Column<long>(type: "bigint", nullable: true),
                    NaturalidadeMunicipioId = table.Column<long>(type: "bigint", nullable: true),
                    NomeMae = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NomePai = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pcd = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DataObito = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TipoPessoaChave = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Documento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Telefone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SituacaoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacaoImovel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Situacoes", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoContatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContatos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoImovel", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TipoUnidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoUnidade", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsuarioEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEnderecos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoSetores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Hierarquia = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SetorPaiId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                        column: x => x.SetorPaiId,
                        principalTable: "OrganizacaoSetores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetores_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sigla = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    UnidadePaiId = table.Column<long>(type: "bigint", nullable: true),
                    Cnpj = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
                    ResponsavelPessoaId = table.Column<long>(type: "bigint", nullable: true),
                    DataFundacao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DataExtincao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    HierarquiaCodigo = table.Column<string>(type: "varchar(600)", maxLength: 600, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HierarquiaNome = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nivel = table.Column<short>(type: "smallint", nullable: true),
                    ValidoDe = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ValidoAte = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoUnidades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidades_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AdministradorDoSistema = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Perfis_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ufs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sigla = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PaisId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ufs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ufs_Paises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "Paises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenhaHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UltimoAcessoOrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    UltimoAcessoOrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    UltimoAcessoSetorId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuarios_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidadeSetores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoUnidadeSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidadeSetores_OrganizacaoUnidades_OrganizacaoUni~",
                        column: x => x.OrganizacaoUnidadeId,
                        principalTable: "OrganizacaoUnidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PerfilId = table.Column<long>(type: "bigint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Municipios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CodigoIbge = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UfId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Municipios_Ufs_UfId",
                        column: x => x.UfId,
                        principalTable: "Ufs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PerfilUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    PerfilId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilUsuarios_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PerfilUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SetorUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    SetorId = table.Column<long>(type: "bigint", nullable: false),
                    HabilitarPermissoesNegativas = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Padrao = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetorUsuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetorUsuarios_OrganizacaoSetores_SetorId",
                        column: x => x.SetorId,
                        principalTable: "OrganizacaoSetores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SetorUsuarios_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bairros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MunicipioId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bairros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bairros_Municipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "Municipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Logradouros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BairroId = table.Column<long>(type: "bigint", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logradouros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logradouros_Bairros_BairroId",
                        column: x => x.BairroId,
                        principalTable: "Bairros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Ceps",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ImovelId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ceps", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    CepId = table.Column<long>(type: "bigint", nullable: false),
                    Complemento = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_Ceps_CepId",
                        column: x => x.CepId,
                        principalTable: "Ceps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enderecos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Imoveis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Cadastro = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LogradouroId = table.Column<long>(type: "bigint", nullable: true),
                    CepId = table.Column<long>(type: "bigint", nullable: true),
                    Numero = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Complemento = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    InscricaoImobiliaria = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TipoImovelId = table.Column<long>(type: "bigint", nullable: true),
                    Latitude = table.Column<double>(type: "double", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInsercao = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Imoveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imoveis_Ceps_CepId",
                        column: x => x.CepId,
                        principalTable: "Ceps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Imoveis_Logradouros_LogradouroId",
                        column: x => x.LogradouroId,
                        principalTable: "Logradouros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Bairros_MunicipioId",
                table: "Bairros",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ceps_ImovelId",
                table: "Ceps",
                column: "ImovelId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_CepId",
                table: "Enderecos",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_UsuarioId",
                table: "Enderecos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Funcoes_OrganizacaoId",
                table: "Funcoes",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_CepId",
                table: "Imoveis",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_LogradouroId",
                table: "Imoveis",
                column: "LogradouroId");

            migrationBuilder.CreateIndex(
                name: "IX_Logradouros_BairroId",
                table: "Logradouros",
                column: "BairroId");

            migrationBuilder.CreateIndex(
                name: "IX_Municipios_UfId",
                table: "Municipios",
                column: "UfId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_OrganizacaoId",
                table: "OrganizacaoUnidades",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidadeSetores_OrganizacaoUnidadeId",
                table: "OrganizacaoUnidadeSetores",
                column: "OrganizacaoUnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_OrganizacaoId",
                table: "PerfilPermissoes",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId_Nome",
                table: "PerfilPermissoes",
                columns: new[] { "PerfilId", "Nome" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PerfilUsuarios_PerfilId",
                table: "PerfilUsuarios",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilUsuarios_UsuarioId",
                table: "PerfilUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_OrganizacaoId",
                table: "Perfis",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SetorUsuarios_SetorId",
                table: "SetorUsuarios",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_SetorUsuarios_UsuarioId",
                table: "SetorUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Ufs_PaisId",
                table: "Ufs",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps",
                column: "ImovelId",
                principalTable: "Imoveis",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bairros_Municipios_MunicipioId",
                table: "Bairros");

            migrationBuilder.DropForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps");

            migrationBuilder.DropTable(
                name: "ContatoRelacionamentos");

            migrationBuilder.DropTable(
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "DocumentoRelacionamentos");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "DocumentoTipos");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "NaturezasJuridicas");

            migrationBuilder.DropTable(
                name: "NiveisGoverno");

            migrationBuilder.DropTable(
                name: "OrganizacaoEnderecos");

            migrationBuilder.DropTable(
                name: "OrganizacaoSetorEnderecos");

            migrationBuilder.DropTable(
                name: "OrganizacaoUnidadeEnderecos");

            migrationBuilder.DropTable(
                name: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropTable(
                name: "OrquestracaoFluxoProcessos");

            migrationBuilder.DropTable(
                name: "PerfilPermissoes");

            migrationBuilder.DropTable(
                name: "PerfilUsuarios");

            migrationBuilder.DropTable(
                name: "PessoaEnderecos");

            migrationBuilder.DropTable(
                name: "SetorUsuarios");

            migrationBuilder.DropTable(
                name: "SituacaoImovel");

            migrationBuilder.DropTable(
                name: "Situacoes");

            migrationBuilder.DropTable(
                name: "TipoContatos");

            migrationBuilder.DropTable(
                name: "TipoEnderecos");

            migrationBuilder.DropTable(
                name: "TipoImovel");

            migrationBuilder.DropTable(
                name: "TipoUnidade");

            migrationBuilder.DropTable(
                name: "UsuarioEnderecos");

            migrationBuilder.DropTable(
                name: "OrganizacaoUnidades");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropTable(
                name: "OrganizacaoSetores");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Organizacoes");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Municipios");

            migrationBuilder.DropTable(
                name: "Ufs");

            migrationBuilder.DropTable(
                name: "Paises");

            migrationBuilder.DropTable(
                name: "Imoveis");

            migrationBuilder.DropTable(
                name: "Ceps");

            migrationBuilder.DropTable(
                name: "Logradouros");

            migrationBuilder.DropTable(
                name: "Bairros");
        }
    }
}
