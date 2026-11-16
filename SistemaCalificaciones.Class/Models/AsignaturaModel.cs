using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Asignaturas")]
public class AsignaturaModel : IEntity
{
    [Column("id_asignatura")]
    public int Id { get; set; }

    [Required(ErrorMessage = "La clave es obligatoria")]
    [StringLength(20)]
    [Display(Name = "Clave")]
    [Column("clave_asignatura")]
    public string ClaveAsignatura { get; set; } = null!;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    [Column("nombre_asignatura")]
    public string NombreAsignatura { get; set; } = null!;

    [Range(0, 20, ErrorMessage = "Los créditos deben estar entre 0 y 20")]
    [Display(Name = "Créditos")]
    [Column("creditos")]
    public int Creditos { get; set; }

    [Range(1, 12, ErrorMessage = "El semestre debe estar entre 1 y 12")]
    [Display(Name = "Semestre")]
    [Column("semestre")]
    public int Semestre { get; set; }

    [Range(1, 15, ErrorMessage = "Las unidades deben estar entre 1 y 15")]
    [Display(Name = "Unidades")]
    [Column("Unidades")]
    public int Unidades { get; set; }

    [Display(Name = "Activa")]
    [Column("estatus")]
    public bool Estatus { get; set; } = true;
}