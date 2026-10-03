using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Estudiante_Grupo")]
public class EstudianteGrupoModel : IEntity
{
    [Column("id_estudiante_grupo")]
    public int Id { get; set; }

    [Column("id_estudiante")]
    public int EstudianteId { get; set; }
    [ForeignKey(nameof(EstudianteId))]
    public EstudianteModel Estudiante { get; set; } = null!;

    [Column("id_grupo")]
    public int GrupoId { get; set; }
    [ForeignKey(nameof(GrupoId))]
    public GrupoModel Grupo { get; set; } = null!;
}