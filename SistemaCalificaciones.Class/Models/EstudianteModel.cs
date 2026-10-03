using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Estudiantes")]
public class EstudianteModel : IEntity
{
    [Column("id_estudiante")]
    public int Id { get; set; }

    [Required, StringLength(20)]
    [Column("matricula")]
    public string Matricula { get; set; } = null!;

    [Required, StringLength(50)]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required, StringLength(50)]
    [Column("apellido_paterno")]
    public string ApellidoPaterno { get; set; } = null!;

    [Required, StringLength(50)]
    [Column("apellido_materno")]
    public string ApellidoMaterno { get; set; } = null!;

    [Required, StringLength(100), EmailAddress(ErrorMessage = "Correo inválido")]
    [Column("correo")]
    public string Correo { get; set; } = null!;

    [Column("estatus")]
    public bool Estatus { get; set; } = true;
}