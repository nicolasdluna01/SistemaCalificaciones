using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Grupos")]
public class GrupoModel : IEntity
{
    [Column("id_grupo")]
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre del grupo es obligatorio")]
    [StringLength(20)]
    [Display(Name = "Grupo")]
    [Column("nombre_grupo")]
    public string NombreGrupo { get; set; } = null!;

    [Range(1, 12, ErrorMessage = "El semestre debe estar entre 1 y 12")]
    [Display(Name = "Semestre")]
    [Column("semestre")]
    public int Semestre { get; set; }

    [Required(ErrorMessage = "El periodo es obligatorio")]
    [StringLength(20)]
    [Display(Name = "Periodo")]
    [Column("periodo")]
    public string Periodo { get; set; } = null!;

    [Required(ErrorMessage = "Selecciona un turno")]
    [StringLength(20)]
    [Display(Name = "Turno")]
    [Column("turno")]
    public string Turno { get; set; } = null!;

    [Display(Name = "Activo")]
    [Column("estatus")]
    public bool Estatus { get; set; } = true;
}