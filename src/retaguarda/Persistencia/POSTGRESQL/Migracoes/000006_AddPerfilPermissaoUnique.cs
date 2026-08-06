using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;

#nullable disable

namespace Retaguarda.Persistencia.POSTGRESQL.Migracoes
{
    [DbContext(typeof(Retaguarda.Persistencia.POSTGRESQL.ApplicationDbContext))]
    [Migration("20260804120001_AddPerfilPermissaoUnique")]
    public partial class AddPerfilPermissaoUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_PerfilPermissoes_PerfilId_Nome",
                table: "PerfilPermissoes",
                columns: new[] { "PerfilId", "Nome" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PerfilPermissoes_PerfilId_Nome",
                table: "PerfilPermissoes");
        }
    }
}




