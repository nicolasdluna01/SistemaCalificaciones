using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCalificaciones.Class.Migrations
{
    /// <inheritdoc />
    public partial class ClaveAsignaturaUnica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Asignaturas_clave_asignatura",
                table: "Asignaturas",
                column: "clave_asignatura",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Asignaturas_clave_asignatura",
                table: "Asignaturas");
        }
    }
}
