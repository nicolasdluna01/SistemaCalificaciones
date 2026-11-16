using System.ComponentModel.DataAnnotations;

namespace SistemaCalificaciones.Web.Models;

public class DocenteViewModel
{
    public int Id { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Ingresa un número de empleado válido")]
    [Display(Name = "Número de empleado")]
    public int NumeroEmpleado { get; set; }

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

    [Required(ErrorMessage = "El usuario es obligatorio")]
    [StringLength(30)]
    [RegularExpression(@"^[a-zA-Z0-9._-]+$", ErrorMessage = "Solo letras, números, punto, guion y guion bajo")]
    [Display(Name = "Usuario")]
    public string Usuario { get; set; } = string.Empty;

    // Solo se usan al crear
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string? Contrasena { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Contrasena), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string? ConfirmarContrasena { get; set; }

    [Display(Name = "Activo")]
    public bool Estatus { get; set; } = true;
}

public class RestablecerContrasenaViewModel
{
    public int Id { get; set; }
    public string NombreDocente { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa la nueva contraseña")]
    [DataType(DataType.Password)]
    [Display(Name = "Nueva contraseña")]
    public string NuevaContrasena { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    [Compare(nameof(NuevaContrasena), ErrorMessage = "Las contraseñas no coinciden")]
    [Display(Name = "Confirmar contraseña")]
    public string? Confirmar { get; set; }
}