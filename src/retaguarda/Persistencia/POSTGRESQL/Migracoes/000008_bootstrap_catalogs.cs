using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    public partial class bootstrap_catalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps");

            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Logradouros_LogradouroId",
                table: "Imoveis");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Usuarios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Usuarios",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Usuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Usuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Ufs",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Ufs",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Ufs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Ufs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Ufs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "SetorUsuarios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "SetorUsuarios",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "SetorUsuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "SetorUsuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "SetorUsuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "Pessoas",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataNascimento",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataObito",
                table: "Pessoas",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "EstadoCivilId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Pessoas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Pessoas",
                type: "text",
                nullable: false);

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

            migrationBuilder.AddColumn<string>(
                name: "NomeMae",
                table: "Pessoas",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "NomePai",
                table: "Pessoas",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "NomeSocial",
                table: "Pessoas",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "Pcd",
                table: "Pessoas",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "SexoId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Pessoas",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Pessoas",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Perfis",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Perfis",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Perfis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Perfis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Perfis",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "PerfilUsuarios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "PerfilUsuarios",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "PerfilUsuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "PerfilUsuarios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "PerfilUsuarios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "PerfilPermissoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "PerfilPermissoes",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "PerfilPermissoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "PerfilPermissoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "PerfilPermissoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Paises",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Paises",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Paises",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Paises",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Paises",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "Organizacoes",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "Organizacoes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "Organizacoes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "Organizacoes",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierarquiaCodigo",
                table: "Organizacoes",
                type: "varchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Organizacoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Organizacoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InscricaoEstadual",
                table: "Organizacoes",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InscricaoMunicipal",
                table: "Organizacoes",
                type: "varchar(50)",
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
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

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
                name: "RazaoSocial",
                table: "Organizacoes",
                type: "varchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sigla",
                table: "Organizacoes",
                type: "varchar(30)",
                maxLength: 30,
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

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Organizacoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Organizacoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "OrganizacaoUnidadeSetores",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoUnidadeSetores",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoUnidadeSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoUnidadeSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "OrganizacaoUnidadeSetores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "OrganizacaoUnidades",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Codigo",
                table: "OrganizacaoUnidades",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataExtincao",
                table: "OrganizacaoUnidades",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DataFundacao",
                table: "OrganizacaoUnidades",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HierarquiaCodigo",
                table: "OrganizacaoUnidades",
                type: "varchar(600)",
                maxLength: 600,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HierarquiaNome",
                table: "OrganizacaoUnidades",
                type: "varchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "OrganizacaoUnidades",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoUnidades",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<short>(
                name: "Nivel",
                table: "OrganizacaoUnidades",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ResponsavelPessoaId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sigla",
                table: "OrganizacaoUnidades",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddColumn<long>(
                name: "UnidadePaiId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoAte",
                table: "OrganizacaoUnidades",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidoDe",
                table: "OrganizacaoUnidades",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "OrganizacaoUnidades",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "OrganizacaoSetores",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoSetores",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "OrganizacaoSetores",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Municipios",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Municipios",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Municipios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Municipios",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Municipios",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Logradouros",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Logradouros",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Logradouros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Logradouros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Logradouros",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "LogradouroId",
                table: "Imoveis",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CepId",
                table: "Imoveis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Complemento",
                table: "Imoveis",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Imoveis",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Imoveis",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "InscricaoImobiliaria",
                table: "Imoveis",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Imoveis",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Imoveis",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Imoveis",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "SituacaoId",
                table: "Imoveis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TipoImovelId",
                table: "Imoveis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Imoveis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Imoveis",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Imoveis",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Funcoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Funcoes",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Funcoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Funcoes",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Funcoes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Enderecos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Enderecos",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Enderecos",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Enderecos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "ImovelId",
                table: "Ceps",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Ceps",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Ceps",
                type: "text",
                nullable: false);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Ceps",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Ceps",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Ceps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "IdentificadorUnico",
                table: "Bairros",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "IdentificadorUnicoAmigavel",
                table: "Bairros",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UsuarioAlteracaoId",
                table: "Bairros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UsuarioInsercaoId",
                table: "Bairros",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Versao",
                table: "Bairros",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContatoRelacionamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoContatoId = table.Column<long>(type: "bigint", nullable: true),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    ContatoValor = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    SetorId = table.Column<long>(type: "bigint", nullable: true),
                    Ativo = table.Column<bool>(type: "boolean", nullable: false),
                    UsuarioInsercaoId = table.Column<long>(type: "bigint", nullable: true),
                    UsuarioAlteracaoId = table.Column<long>(type: "bigint", nullable: true),
                    Versao = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentoRelacionamentos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Documentos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocumentoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    Numero = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    Digito = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        ,
                    OrgaoEmissor = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        ,
                    UfEmissor = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false)
                        ,
                    DataEmissao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DataValidade = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Principal = table.Column<bool>(type: "boolean", nullable: false),
                    Validado = table.Column<bool>(type: "boolean", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "DocumentoTipos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_DocumentoTipos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NaturezasJuridicas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_NaturezasJuridicas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NiveisGoverno",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_NiveisGoverno", x => x.Id);
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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                });

            migrationBuilder.CreateTable(
                name: "SituacaoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_SituacaoImovel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Situacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                name: "TipoContatos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_TipoContatos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_TipoEnderecos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoImovel",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_TipoImovel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TipoUnidade",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Codigo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        ,
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_TipoUnidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEnderecos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoId = table.Column<long>(type: "bigint", nullable: false),
                    EnderecoTipoId = table.Column<long>(type: "bigint", nullable: true),
                    EnderecoPrincipal = table.Column<bool>(type: "boolean", nullable: false),
                    IdentificadorUnico = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentificadorUnicoAmigavel = table.Column<string>(type: "text", nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
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
                    table.PrimaryKey("PK_UsuarioEnderecos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Imoveis_CepId",
                table: "Imoveis",
                column: "CepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps",
                column: "ImovelId",
                principalTable: "Imoveis",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Ceps_CepId",
                table: "Imoveis",
                column: "CepId",
                principalTable: "Ceps",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Logradouros_LogradouroId",
                table: "Imoveis",
                column: "LogradouroId",
                principalTable: "Logradouros",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps");

            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Ceps_CepId",
                table: "Imoveis");

            migrationBuilder.DropForeignKey(
                name: "FK_Imoveis_Logradouros_LogradouroId",
                table: "Imoveis");

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
                name: "PessoaEnderecos");

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

            migrationBuilder.DropIndex(
                name: "IX_Imoveis_CepId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Ufs");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Ufs");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Ufs");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Ufs");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Ufs");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "SetorUsuarios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "SetorUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "SetorUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "SetorUsuarios");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "SetorUsuarios");

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
                name: "EstadoCivilId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NacionalidadePaisId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NaturalidadeMunicipioId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeMae",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomePai",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "NomeSocial",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Pcd",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "SexoId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Pessoas");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Perfis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "PerfilUsuarios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "PerfilUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "PerfilUsuarios");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "PerfilUsuarios");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "PerfilUsuarios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "PerfilPermissoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Paises");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "DataExtincao",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "HierarquiaCodigo",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
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
                name: "OrganizacaoPaiId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "OrganizacaoRaizId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "RazaoSocial",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Sigla",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "TipoOrganizacaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Organizacoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "OrganizacaoUnidadeSetores");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "Codigo",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "DataExtincao",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "DataFundacao",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "HierarquiaCodigo",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "HierarquiaNome",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "Nivel",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ResponsavelPessoaId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "Sigla",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "TipoUnidadeId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "UnidadePaiId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ValidoAte",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "ValidoDe",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "OrganizacaoUnidades");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "OrganizacaoSetores");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Municipios");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Logradouros");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Logradouros");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Logradouros");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Logradouros");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Logradouros");

            migrationBuilder.DropColumn(
                name: "CepId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Complemento",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "InscricaoImobiliaria",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "SituacaoId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "TipoImovelId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Imoveis");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Funcoes");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Enderecos");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Ceps");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Ceps");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Ceps");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Ceps");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Ceps");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnico",
                table: "Bairros");

            migrationBuilder.DropColumn(
                name: "IdentificadorUnicoAmigavel",
                table: "Bairros");

            migrationBuilder.DropColumn(
                name: "UsuarioAlteracaoId",
                table: "Bairros");

            migrationBuilder.DropColumn(
                name: "UsuarioInsercaoId",
                table: "Bairros");

            migrationBuilder.DropColumn(
                name: "Versao",
                table: "Bairros");

            migrationBuilder.AlterColumn<long>(
                name: "LogradouroId",
                table: "Imoveis",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ImovelId",
                table: "Ceps",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ceps_Imoveis_ImovelId",
                table: "Ceps",
                column: "ImovelId",
                principalTable: "Imoveis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Imoveis_Logradouros_LogradouroId",
                table: "Imoveis",
                column: "LogradouroId",
                principalTable: "Logradouros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}





