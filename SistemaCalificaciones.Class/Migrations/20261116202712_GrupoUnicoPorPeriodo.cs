using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCalificaciones.Class.Migrations
{
    /// <inheritdoc />
    public partial class GrupoUnicoPorPeriodo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Grupos_nombre_grupo_periodo",
                table: "Grupos",
                columns: new[] { "nombre_grupo", "periodo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grupos_nombre_grupo_periodo",
                table: "Grupos");
        }
    }
}
