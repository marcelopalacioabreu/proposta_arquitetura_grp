using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnderecoPaises",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Codigo = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoPaises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrquestracaoFluxoProcessos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descricao = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    WorkflowDefinitionId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    WorkflowVersion = table.Column<int>(type: "integer", nullable: true),
                    WorkflowJson = table.Column<string>(type: "text", nullable: true),
                    WorkflowNome = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_OrquestracaoFluxoProcessos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    AdministradorDoSistema = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
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
                    table.PrimaryKey("PK_Perfis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Contexto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Situacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Contexto = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoUFs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Sigla = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    PaisId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoUFs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoUFs_EnderecoPaises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "EnderecoPaises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PerfilId = table.Column<long>(type: "bigint", nullable: false),
                    Chave = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
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
                    table.PrimaryKey("PK_PerfilPermissoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PerfilPermissoes_Perfis_PerfilId",
                        column: x => x.PerfilId,
                        principalTable: "Perfis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pessoas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoPessoa = table.Column<string>(type: "text", nullable: false),
                    Discriminator = table.Column<int>(type: "integer", nullable: false),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    NomeSocial = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Cpf = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Sexo = table.Column<string>(type: "text", nullable: true),
                    EstadoCivil = table.Column<string>(type: "text", nullable: true),
                    NomeMae = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    NomePai = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    Pcd = table.Column<bool>(type: "boolean", nullable: true),
                    DataObito = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RazaoSocial = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    NomeFantasia = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    DataFundacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExtincao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Cnpj = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    Anotacoes = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    InscricaoEstadual = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    InscricaoMunicipal = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Pessoas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pessoas_Situacoes_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Situacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoContatoId = table.Column<long>(type: "bigint", nullable: true),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ContatoValor = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TipoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Contatos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contatos_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    Numero = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Digito = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    OrgaoEmissor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UfEmissor = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    DataEmissao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataValidade = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Principal = table.Column<bool>(type: "boolean", nullable: false),
                    Validado = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    TipoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Documentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documentos_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoMunicipios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CodigoIbge = table.Column<string>(type: "text", nullable: false),
                    UfId = table.Column<long>(type: "bigint", nullable: false),
                    CepId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_EnderecoMunicipios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoMunicipios_EnderecoUFs_UfId",
                        column: x => x.UfId,
                        principalTable: "EnderecoUFs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Organizacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sigla = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Nivel = table.Column<long>(type: "bigint", nullable: true),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    TipoId = table.Column<long>(type: "bigint", nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoPaiId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoRaizId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Organizacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizacoes_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizacoes_Situacoes_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Situacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Organizacoes_Tipos_TipoId",
                        column: x => x.TipoId,
                        principalTable: "Tipos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SenhaHash = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    UltimoAcessoOrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    UltimoAcessoOrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    UltimoAcessoSetorId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuarios_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContatoRelacionamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContatoId = table.Column<long>(type: "bigint", nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoRelacionamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContatoRelacionamentos_Contatos_ContatoId",
                        column: x => x.ContatoId,
                        principalTable: "Contatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContatoRelacionamentos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DocumentoRelacionamentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentoId = table.Column<long>(type: "bigint", nullable: false),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoRelacionamentos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentoRelacionamentos_Documentos_DocumentoId",
                        column: x => x.DocumentoId,
                        principalTable: "Documentos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DocumentoRelacionamentos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EnderecoBairros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    MunicipioId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoBairros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoBairros_EnderecoMunicipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "EnderecoMunicipios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidades",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Sigla = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Descricao = table.Column<string>(type: "text", nullable: false),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    UnidadePaiId = table.Column<long>(type: "bigint", nullable: true),
                    TipoId = table.Column<long>(type: "bigint", nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
                    ResponsavelId = table.Column<long>(type: "bigint", nullable: true),
                    PessoaId = table.Column<long>(type: "bigint", nullable: true),
                    Nivel = table.Column<long>(type: "bigint", nullable: true),
                    DataFundacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DataExtincao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TipoId1 = table.Column<long>(type: "bigint", nullable: true),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidades_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidades_Situacoes_SituacaoId",
                        column: x => x.SituacaoId,
                        principalTable: "Situacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidades_Tipos_TipoId1",
                        column: x => x.TipoId1,
                        principalTable: "Tipos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PerfilUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    PerfilId = table.Column<long>(type: "bigint", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "EnderecoLogradouros",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    BairroId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoLogradouros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoLogradouros_EnderecoBairros_BairroId",
                        column: x => x.BairroId,
                        principalTable: "EnderecoBairros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoSetores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CodigoHierarquico = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ResponsavelSetorId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_OrganizacaoSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetores_OrganizacaoUnidades_OrganizacaoUnidadeId",
                        column: x => x.OrganizacaoUnidadeId,
                        principalTable: "OrganizacaoUnidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidadeSetores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("PK_OrganizacaoUnidadeSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidadeSetores_OrganizacaoUnidades_OrganizacaoUn~",
                        column: x => x.OrganizacaoUnidadeId,
                        principalTable: "OrganizacaoUnidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EnderecoCEPs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LogradouroId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_EnderecoCEPs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnderecoCEPs_EnderecoLogradouros_LogradouroId",
                        column: x => x.LogradouroId,
                        principalTable: "EnderecoLogradouros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetorUsuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    SetorId = table.Column<long>(type: "bigint", nullable: false),
                    HabilitarPermissoesNegativas = table.Column<bool>(type: "boolean", nullable: false),
                    Padrao = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "Enderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PaisId = table.Column<long>(type: "bigint", nullable: true),
                    UfId = table.Column<long>(type: "bigint", nullable: true),
                    MunicipioId = table.Column<long>(type: "bigint", nullable: true),
                    BairroId = table.Column<long>(type: "bigint", nullable: true),
                    LogradouroId = table.Column<long>(type: "bigint", nullable: true),
                    CepId = table.Column<long>(type: "bigint", nullable: false),
                    Complemento = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
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
                    table.PrimaryKey("PK_Enderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoBairros_BairroId",
                        column: x => x.BairroId,
                        principalTable: "EnderecoBairros",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoCEPs_CepId",
                        column: x => x.CepId,
                        principalTable: "EnderecoCEPs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoLogradouros_LogradouroId",
                        column: x => x.LogradouroId,
                        principalTable: "EnderecoLogradouros",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoMunicipios_MunicipioId",
                        column: x => x.MunicipioId,
                        principalTable: "EnderecoMunicipios",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoPaises_PaisId",
                        column: x => x.PaisId,
                        principalTable: "EnderecoPaises",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Enderecos_EnderecoUFs_UfId",
                        column: x => x.UfId,
                        principalTable: "EnderecoUFs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Imoveis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cadastro = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: true),
                    InscricaoImobiliaria = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    TipoImovelId = table.Column<long>(type: "bigint", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    SituacaoId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Imoveis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Imoveis_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoEnderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoEnderecos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizacaoEnderecos_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoSetorEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoSetorId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_OrganizacaoSetorEnderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetorEnderecos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetorEnderecos_OrganizacaoSetores_OrganizacaoSet~",
                        column: x => x.OrganizacaoSetorId,
                        principalTable: "OrganizacaoSetores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizacaoUnidadeEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoUnidadeId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false),
                    DataInsercao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoUnidadeEnderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidadeEnderecos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizacaoUnidadeEnderecos_OrganizacaoUnidades_Organizacao~",
                        column: x => x.OrganizacaoUnidadeId,
                        principalTable: "OrganizacaoUnidades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PessoaEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PessoaId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("PK_PessoaEnderecos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PessoaEnderecos_Enderecos_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Enderecos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PessoaEnderecos_Pessoas_PessoaId",
                        column: x => x.PessoaId,
                        principalTable: "Pessoas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContatoRelacionamentos_ContatoId",
                table: "ContatoRelacionamentos",
                column: "ContatoId");

            migrationBuilder.CreateIndex(
                name: "IX_ContatoRelacionamentos_PessoaId",
                table: "ContatoRelacionamentos",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contatos_TipoId",
                table: "Contatos",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoRelacionamentos_DocumentoId",
                table: "DocumentoRelacionamentos",
                column: "DocumentoId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentoRelacionamentos_PessoaId",
                table: "DocumentoRelacionamentos",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Documentos_TipoId",
                table: "Documentos",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoBairros_MunicipioId",
                table: "EnderecoBairros",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoCEPs_LogradouroId",
                table: "EnderecoCEPs",
                column: "LogradouroId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoLogradouros_BairroId",
                table: "EnderecoLogradouros",
                column: "BairroId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoMunicipios_UfId",
                table: "EnderecoMunicipios",
                column: "UfId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_BairroId",
                table: "Enderecos",
                column: "BairroId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_CepId",
                table: "Enderecos",
                column: "CepId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_LogradouroId",
                table: "Enderecos",
                column: "LogradouroId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_MunicipioId",
                table: "Enderecos",
                column: "MunicipioId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_PaisId",
                table: "Enderecos",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Enderecos_UfId",
                table: "Enderecos",
                column: "UfId");

            migrationBuilder.CreateIndex(
                name: "IX_EnderecoUFs_PaisId",
                table: "EnderecoUFs",
                column: "PaisId");

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_EnderecoId",
                table: "Imoveis",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoEnderecos_EnderecoId",
                table: "OrganizacaoEnderecos",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoEnderecos_OrganizacaoId",
                table: "OrganizacaoEnderecos",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetorEnderecos_EnderecoId",
                table: "OrganizacaoSetorEnderecos",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetorEnderecos_OrganizacaoSetorId",
                table: "OrganizacaoSetorEnderecos",
                column: "OrganizacaoSetorId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoUnidadeId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoUnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidadeEnderecos_EnderecoId",
                table: "OrganizacaoUnidadeEnderecos",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidadeEnderecos_OrganizacaoUnidadeId",
                table: "OrganizacaoUnidadeEnderecos",
                column: "OrganizacaoUnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_OrganizacaoId",
                table: "OrganizacaoUnidades",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_PessoaId",
                table: "OrganizacaoUnidades",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_SituacaoId",
                table: "OrganizacaoUnidades",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_TipoId1",
                table: "OrganizacaoUnidades",
                column: "TipoId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidadeSetores_OrganizacaoUnidadeId",
                table: "OrganizacaoUnidadeSetores",
                column: "OrganizacaoUnidadeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizacoes_PessoaId",
                table: "Organizacoes",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizacoes_SituacaoId",
                table: "Organizacoes",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizacoes_TipoId",
                table: "Organizacoes",
                column: "TipoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId_Chave",
                table: "PerfilPermissoes",
                columns: new[] { "PerfilId", "Chave" },
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
                name: "IX_PessoaEnderecos_EnderecoId",
                table: "PessoaEnderecos",
                column: "EnderecoId");

            migrationBuilder.CreateIndex(
                name: "IX_PessoaEnderecos_PessoaId",
                table: "PessoaEnderecos",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Pessoas_SituacaoId",
                table: "Pessoas",
                column: "SituacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_SetorUsuarios_SetorId",
                table: "SetorUsuarios",
                column: "SetorId");

            migrationBuilder.CreateIndex(
                name: "IX_SetorUsuarios_UsuarioId",
                table: "SetorUsuarios",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Codigo_Contexto_Unico",
                table: "Situacoes",
                columns: new[] { "Codigo", "Contexto", "OrganizacaoId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_Situacoes_Contexto_Ativo",
                table: "Situacoes",
                columns: new[] { "OrganizacaoId", "Contexto", "Ativo" });

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
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContatoRelacionamentos");

            migrationBuilder.DropTable(
                name: "DocumentoRelacionamentos");

            migrationBuilder.DropTable(
                name: "Imoveis");

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
                name: "Contatos");

            migrationBuilder.DropTable(
                name: "Documentos");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropTable(
                name: "Enderecos");

            migrationBuilder.DropTable(
                name: "OrganizacaoSetores");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "EnderecoCEPs");

            migrationBuilder.DropTable(
                name: "OrganizacaoUnidades");

            migrationBuilder.DropTable(
                name: "EnderecoLogradouros");

            migrationBuilder.DropTable(
                name: "Organizacoes");

            migrationBuilder.DropTable(
                name: "EnderecoBairros");

            migrationBuilder.DropTable(
                name: "Pessoas");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropTable(
                name: "EnderecoMunicipios");

            migrationBuilder.DropTable(
                name: "Situacoes");

            migrationBuilder.DropTable(
                name: "EnderecoUFs");

            migrationBuilder.DropTable(
                name: "EnderecoPaises");
        }
    }
}
