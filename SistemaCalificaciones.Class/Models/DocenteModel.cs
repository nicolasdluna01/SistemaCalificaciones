using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace SistemaCalificaciones.Class.Models;

[Table("Docentes")]
public class DocenteModel : IEntity
{
    [Column("id_docente")]
    public int Id { get; set; }

    [Column("numero_empleado")]
    public int NumeroEmpleado { get; set; }

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

    // Debe coincidir con el UserName de la cuenta de Identity
    [Required, StringLength(30)]
    [Column("usuario")]
    public string Usuario { get; set; } = null!;

    // Copia del hash que genera Identity (se conserva para respetar el diagrama).
    // La autenticación real usa AspNetUsers.PasswordHash.
    [Required, StringLength(255)]
    [Column("contrasena")]
    public string Contrasena { get; set; } = null!;

    [Column("estatus")]
    public bool Estatus { get; set; } = true;

    // Vínculo con la cuenta de Identity (columna extra respecto al diagrama)
    [Column("id_usuario")]
    public string? UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public IdentityUser? User { get; set; }
}