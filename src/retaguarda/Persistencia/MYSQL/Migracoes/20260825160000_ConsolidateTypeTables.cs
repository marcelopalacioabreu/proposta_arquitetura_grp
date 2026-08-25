using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Retaguarda.Persistencia.MYSQL.Migracoes
{
    /// <inheritdoc />
    public partial class ConsolidateTypeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Step 1: Add new columns to Tipos if not exists
            migrationBuilder.AddColumn<string>(
                name: "Contexto",
                table: "Tipos",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Tipos",
                type: "varchar(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ordem",
                table: "Tipos",
                type: "int",
                nullable: true);

            // Step 2: Migrate data from TipoEndereco to Tipos
            migrationBuilder.Sql(
                @"INSERT INTO Tipos (Codigo, Nome, Contexto, Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'ENDERECO', Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM TipoEnderecos
                ON DUPLICATE KEY UPDATE Codigo=Codigo");

            // Step 3: Migrate data from TipoContato to Tipos
            migrationBuilder.Sql(
                @"INSERT INTO Tipos (Codigo, Nome, Contexto, Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'CONTATO', Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM TipoContatos
                ON DUPLICATE KEY UPDATE Codigo=Codigo");

            // Step 4: Migrate data from TipoUnidade to Tipos
            migrationBuilder.Sql(
                @"INSERT INTO Tipos (Codigo, Nome, Contexto, Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'UNIDADE', Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM TipoUnidades
                ON DUPLICATE KEY UPDATE Codigo=Codigo");

            // Step 5: Migrate data from TipoImovel to Tipos
            migrationBuilder.Sql(
                @"INSERT INTO Tipos (Codigo, Nome, Contexto, Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'IMOVEL', Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM TipoImovels
                ON DUPLICATE KEY UPDATE Codigo=Codigo");

            // Step 6: Migrate data from DocumentoTipo to Tipos
            migrationBuilder.Sql(
                @"INSERT INTO Tipos (Codigo, Nome, Contexto, Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico)
                SELECT Codigo, Nome, 'DOCUMENTO', Descricao, Ordem, Ativo, DataInsercao, DataAlteracao, OrganizacaoId, OrganizacaoUnidadeId, SetorId, UsuarioInsercaoId, UsuarioAlteracaoId, Versao, IdentificadorUnico
                FROM DocumentoTipos
                ON DUPLICATE KEY UPDATE Codigo=Codigo");

            // Step 7: Drop old tables
            migrationBuilder.DropTable(name: "TipoEnderecos");
            migrationBuilder.DropTable(name: "TipoContatos");
            migrationBuilder.DropTable(name: "TipoUnidades");
            migrationBuilder.DropTable(name: "TipoImovels");
            migrationBuilder.DropTable(name: "DocumentoTipos");

            // Step 8: Create composite indices
            migrationBuilder.CreateIndex(
                name: "idx_Tipos_Contexto_Ativo",
                table: "Tipos",
                columns: new[] { "OrganizacaoId", "Contexto", "Ativo" });

            migrationBuilder.CreateIndex(
                name: "idx_Tipos_Codigo_Contexto_Unico",
                table: "Tipos",
                columns: new[] { "Codigo", "Contexto", "OrganizacaoId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Note: Down migration would require recreating the old tables
            // This is intentionally left incomplete as a safety measure
            migrationBuilder.DropIndex(
                name: "idx_Tipos_Contexto_Ativo",
                table: "Tipos");

            migrationBuilder.DropIndex(
                name: "idx_Tipos_Codigo_Contexto_Unico",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Contexto",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Tipos");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "Tipos");
        }
    }
}
