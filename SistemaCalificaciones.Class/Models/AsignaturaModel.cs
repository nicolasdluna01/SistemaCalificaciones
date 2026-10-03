using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Asignaturas")]
public class AsignaturaModel : IEntity
{
    [Column("id_asignatura")]
    public int Id { get; set; }

    [Required, StringLength(20)]
    [Column("clave_asignatura")]
    public string ClaveAsignatura { get; set; } = null!;

    [Required, StringLength(100)]
    [Column("nombre_asignatura")]
    public string NombreAsignatura { get; set; } = null!;

    [Column("creditos")]
    public int Creditos { get; set; }

    [Column("semestre")]
    public int Semestre { get; set; }

    [Column("Unidades")]
    public int Unidades { get; set; }

    [Column("estatus")]
    public bool Estatus { get; set; } = true;
}
