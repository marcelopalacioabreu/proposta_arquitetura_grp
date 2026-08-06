using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    /// <inheritdoc />
    [Migration("000001_InitialCreate")]
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase( );

            migrationBuilder.CreateTable(
                name: "Organizacoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizacoes", x => x.Id);
                } );

            migrationBuilder.CreateTable(
                name: "Funcoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
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
                } );

            migrationBuilder.CreateTable(
                name: "OrganizacaoSetores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizacaoSetores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizacaoSetores_Organizacoes_OrganizacaoId",
                        column: x => x.OrganizacaoId,
                        principalTable: "Organizacoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                } );

            migrationBuilder.CreateTable(
                name: "Perfis",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
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
                } );

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
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
                } );

            migrationBuilder.CreateTable(
                name: "PerfilPermissoes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrganizacaoId = table.Column<long>(type: "bigint", nullable: false),
                    
                    PerfilId = table.Column<long>(type: "bigint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        ,
                    DataInsercao = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    DataAlteracao = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
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
                } );

            migrationBuilder.CreateIndex(
                name: "IX_Funcoes_OrganizacaoId",
                table: "Funcoes",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizacaoSetores_OrganizacaoId",
                table: "OrganizacaoSetores",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_OrganizacaoId",
                table: "PerfilPermissoes",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId",
                table: "PerfilPermissoes",
                column: "PerfilId");

            migrationBuilder.CreateIndex(
                name: "IX_Perfis_OrganizacaoId",
                table: "Perfis",
                column: "OrganizacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_OrganizacaoId",
                table: "Usuarios",
                column: "OrganizacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funcoes");

            migrationBuilder.DropTable(
                name: "OrganizacaoSetores");

            migrationBuilder.DropTable(
                name: "PerfilPermissoes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Perfis");

            migrationBuilder.DropTable(
                name: "Organizacoes");
        }
    }
}




