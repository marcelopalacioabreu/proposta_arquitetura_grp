using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.MYSQL.Migracoes
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
                table: "Pessoas",
                newName: "NomePai");

            migrationBuilder.RenameColumn(
                name: "Hierarquia",
                table: "OrganizacaoSetores",
                newName: "CodigoHierarquico");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Pessoas",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Nivel",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UnidadePaiId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrganizacaoPaiId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "OrganizacaoRaizId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sigla",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoOrganizacaoId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoOrganizacaoUnidadeId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UltimoAcessoSetorId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Pessoas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataObito",
                table: "Pessoas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFantasia",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeMae",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeSocial",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pcd",
                table: "Pessoas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Sexo",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EstadoCivil",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TipoPessoa",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "Pessoas",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "InscricaoMunicipal",
                table: "Pessoas",
                newName: "Anotacoes");

            migrationBuilder.CreateTable(
                name: "TipoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: true),
                    Nome = table.Column<string>(type: "longtext", nullable: true),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AlteradoPor = table.Column<string>(type: "longtext", nullable: true),
                    AlteradoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SituacaoContextos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: true),
                    Descricao = table.Column<string>(type: "longtext", nullable: true),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AlteradoPor = table.Column<string>(type: "longtext", nullable: true),
                    AlteradoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SituacaoContextos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Codigo = table.Column<string>(type: "longtext", nullable: true),
                    Nome = table.Column<string>(type: "longtext", nullable: true),
                    TipoContextoId = table.Column<long>(type: "bigint", nullable: false),
                    TenantId = table.Column<long>(type: "bigint", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AlteradoPor = table.Column<string>(type: "longtext", nullable: true),
                    AlteradoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tipos_TipoContextos_TipoContextoId",
                        column: x => x.TipoContextoId,
                        principalTable: "TipoContextos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tipos_TipoContextoId",
                table: "Tipos",
                column: "TipoContextoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoUnidades_UnidadePaiId",
                table: "OrganizacaoUnidades",
                column: "UnidadePaiId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoUnidades_OrganizacaoUnidades_UnidadePaiId",
                table: "OrganizacaoUnidades",
                column: "UnidadePaiId",
                principalTable: "OrganizacaoUnidades",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizacoes_Organizacoes_OrganizacaoPaiId",
                table: "Organizacoes",
                column: "OrganizacaoPaiId",
                principalTable: "Organizacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Organizacoes_Organizacoes_OrganizacaoRaizId",
                table: "Organizacoes",
                column: "OrganizacaoRaizId",
                principalTable: "Organizacoes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrganizacaoUnidades_OrganizacaoUnidades_UnidadePaiId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizacoes_Organizacoes_OrganizacaoPaiId",
                table: "Organizacoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizacoes_Organizacoes_OrganizacaoRaizId",
                table: "Organizacoes");

            migrationBuilder.DropTable(
                name: "Tipos");

            migrationBuilder.DropTable(
                name: "SituacaoContextos");

            migrationBuilder.DropTable(
                name: "TipoContextos");

            migrationBuilder.DropIndex(
                name: "IX_OrganizacaoUnidades_UnidadePaiId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "OrganizacaoPaiId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "OrganizacaoRaizId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Sigla",
                table: "Organizacoes");

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
                name: "Nivel",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "UnidadePaiId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "InscricaoEstadual",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "DataObito",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeFantasia",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeMae",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeSocial",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Pcd",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "EstadoCivil",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "TipoPessoa",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Anotacoes",
                table: "Pessoas");

            migrationBuilder.RenameColumn(
                name: "NomePai",
                table: "Pessoas",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "CodigoHierarquico",
                table: "OrganizacaoSetores",
                newName: "Hierarquia");

            migrationBuilder.AddColumn<string>(
                name: "Telefone",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NomeFantasia",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RazaoSocial",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

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

            migrationBuilder.AddColumn<long>(
                name: "NivelGovernoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

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

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "Organizacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "Organizacoes",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InscricaoMunicipal",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Organizacoes",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TipoUnidadeId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsavelPessoaId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "OrganizacaoUnidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "OrganizacaoUnidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "OrganizacaoUnidades",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "OrganizacaoUnidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "OrganizacaoUnidades",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SetorPaiId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuarios",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PessoaId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Documento",
                table: "Pessoas",
                type: "longtext",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Pessoas",
                type: "longtext",
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
                type: "longtext",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    AlteradoEm = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AlteradoPor = table.Column<string>(type: "longtext", nullable: true),
                    CriadoEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CriadoPor = table.Column<string>(type: "longtext", nullable: true),
                    Nome = table.Column<string>(type: "longtext", nullable: true),
                    TenantId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funcoes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PessoaId",
                table: "Usuarios",
                column: "PessoaId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId");

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
                name: "IX_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrganizacaoSetores_OrganizacaoSetores_SetorPaiId",
                table: "OrganizacaoSetores",
                column: "SetorPaiId",
                principalTable: "OrganizacaoSetores",
                principalColumn: "Id");

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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Perfis_Organizacoes_OrganizacaoId",
                table: "Perfis",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Organizacoes_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId",
                principalTable: "Organizacoes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Pessoas_PessoaId",
                table: "Usuarios",
                column: "PessoaId",
                principalTable: "Pessoas",
                principalColumn: "Id");
        }
    }
}
