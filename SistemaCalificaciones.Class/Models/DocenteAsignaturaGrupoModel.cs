using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCalificaciones.Class.Models;

[Table("Docente_Asignatura_Grupo")]
public class DocenteAsignaturaGrupoModel : IEntity
{
    [Column("id_docente_asignatura")]
    public int Id { get; set; }

    [Column("id_docente")]
    public int DocenteId { get; set; }
    [ForeignKey(nameof(DocenteId))]
    public DocenteModel Docente { get; set; } = null!;

    [Column("id_asignatura")]
    public int AsignaturaId { get; set; }
    [ForeignKey(nameof(AsignaturaId))]
    public AsignaturaModel Asignatura { get; set; } = null!;

    [Column("id_grupo")]
    public int GrupoId { get; set; }
    [ForeignKey(nameof(GrupoId))]
    public GrupoModel Grupo { get; set; } = null!;
}