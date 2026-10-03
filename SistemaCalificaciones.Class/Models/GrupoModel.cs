using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Grupos")]
public class GrupoModel : IEntity
{
    [Column("id_grupo")]
    public int Id { get; set; }

    [Required, StringLength(20)]
    [Column("nombre_grupo")]
    public string NombreGrupo { get; set; } = null!;

    [Column("semestre")]
    public int Semestre { get; set; }

    [Required, StringLength(20)]
    [Column("periodo")]
    public string Periodo { get; set; } = null!;

    [Required, StringLength(20)]
    [Column("turno")]
    public string Turno { get; set; } = null!;

    [Column("estatus")]
    public bool Estatus { get; set; } = true;
}