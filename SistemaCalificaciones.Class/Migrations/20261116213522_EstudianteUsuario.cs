using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaCalificaciones.Class.Migrations
{
    /// <inheritdoc />
    public partial class EstudianteUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "id_usuario",
                table: "Estudiantes",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Estudiantes_id_usuario",
                table: "Estudiantes",
                column: "id_usuario",
                unique: true,
                filter: "[id_usuario] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Estudiantes_AspNetUsers_id_usuario",
                table: "Estudiantes",
                column: "id_usuario",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Estudiantes_AspNetUsers_id_usuario",
                table: "Estudiantes");

            migrationBuilder.DropIndex(
                name: "IX_Estudiantes_id_usuario",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "id_usuario",
                table: "Estudiantes");
        }
    }
}
