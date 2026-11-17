using System.ComponentModel.DataAnnotations;

namespace SistemaCalificaciones.Web.Models;

public class EstudianteViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "La matrícula es obligatoria")]
    [StringLength(20)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Solo letras, números, punto, guion y guion bajo")]
    [Display(Name = "Matrícula")]
    public string Matricula { get; set; } = string.Empty;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido paterno es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Apellido paterno")]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido materno es obligatorio")]
    [StringLength(50)]
    [Display(Name = "Apellido materno")]
    public string ApellidoMaterno { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Correo inválido")]
    [Display(Name = "Correo")]
    public string Correo { get; set; } = string.Empty;

    [Display(Name = "Activo")]
    public bool Estatus { get; set; } = true;
}