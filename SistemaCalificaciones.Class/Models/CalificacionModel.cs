using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Calificaciones")]
public class CalificacionModel : IEntity
{
    [Column("id_calificacion")]
    public int Id { get; set; }

    [Column("id_estudiante")]
    public int EstudianteId { get; set; }
    [ForeignKey(nameof(EstudianteId))]
    public EstudianteModel Estudiante { get; set; } = null!;

    [Column("id_docente_asignatura")]
    public int DocenteAsignaturaGrupoId { get; set; }
    [ForeignKey(nameof(DocenteAsignaturaGrupoId))]
    public DocenteAsignaturaGrupoModel DocenteAsignaturaGrupo { get; set; } = null!;

    [Column("unidad")]
    public int Unidad { get; set; }

    [Range(0, 100, ErrorMessage = "La calificación debe estar entre 0 y 100")]
    [Column("calificacion", TypeName = "decimal(5,2)")]
    public decimal Calificacion { get; set; }

    [Column("oportunidad")]
    public int Oportunidad { get; set; } = 1;

    [Column("fecha_registro", TypeName = "datetime")]
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}