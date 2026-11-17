using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SistemaCalificaciones.Class.Models;

[Table("Estudiantes")]
public class EstudianteModel : IEntity
{
    [Column("id_estudiante")]
    public int Id { get; set; }

    [Required(ErrorMessage = "La matrícula es obligatoria")]
    [StringLength(20)]
    [Display(Name = "Matrícula")]
    [Column("matricula")]
    public string Matricula { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Nombre")]
    [Column("nombre")]
    public string Nombre { get; set; } = null!;

    [Required(ErrorMessage = "El apellido paterno es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Apellido paterno")]
    [Column("apellido_paterno")]
    public string ApellidoPaterno { get; set; } = null!;

    [Required(ErrorMessage = "El apellido materno es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Apellido materno")]
    [Column("apellido_materno")]
    public string ApellidoMaterno { get; set; } = null!;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Display(Name = "Correo")]
    [Column("correo")]
    public string Correo { get; set; } = null!;

    [Display(Name = "Activo")]
    [Column("estatus")]
    public bool Estatus { get; set; } = true;

    // Vínculo con la cuenta de Identity (columna extra respecto al diagrama)
    [Column("id_usuario")]
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public IdentityUser? User { get; set; }
}