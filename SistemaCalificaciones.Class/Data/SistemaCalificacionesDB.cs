using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Data;

public class SistemaCalificacionesDB : IdentityDbContext<IdentityUser>
{
    public DbSet<EstudianteModel> Estudiantes { get; set; }
    public DbSet<DocenteModel> Docentes { get; set; }
    public DbSet<GrupoModel> Grupos { get; set; }
    public DbSet<AsignaturaModel> Asignaturas { get; set; }
    public DbSet<EstudianteGrupoModel> EstudianteGrupos { get; set; }
    public DbSet<DocenteAsignaturaGrupoModel> DocenteAsignaturaGrupos { get; set; }
    public DbSet<CalificacionModel> Calificaciones { get; set; }

    public SistemaCalificacionesDB(DbContextOptions<SistemaCalificacionesDB> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // En un IdentityDbContext esta llamada va primero
        base.OnModelCreating(builder);

        // La matrícula no debe repetirse (requisito del documento)
        builder.Entity<EstudianteModel>().HasIndex(e => e.Matricula).IsUnique();

        // Estas las agrego por sentido común: no deben repetirse
        builder.Entity<DocenteModel>().HasIndex(d => d.Usuario).IsUnique();
        builder.Entity<DocenteModel>().HasIndex(d => d.NumeroEmpleado).IsUnique();
        builder.Entity<DocenteModel>().HasIndex(d => d.UserId).IsUnique();

        // Evita duplicados en las tablas de relación y en las calificaciones
        builder.Entity<EstudianteGrupoModel>()
            .HasIndex(eg => new { eg.EstudianteId, eg.GrupoId }).IsUnique();
        builder.Entity<DocenteAsignaturaGrupoModel>()
            .HasIndex(d => new { d.DocenteId, d.AsignaturaId, d.GrupoId }).IsUnique();
        builder.Entity<CalificacionModel>()
            .HasIndex(c => new { c.EstudianteId, c.DocenteAsignaturaGrupoId, c.Unidad, c.Oportunidad }).IsUnique();

        // Sin borrado en cascada en NUESTRAS tablas (no tocamos las de Identity):
        // borrar un grupo o un docente no debe borrar calificaciones
        foreach (var fk in builder.Model.GetEntityTypes()
                     .Where(e => typeof(IEntity).IsAssignableFrom(e.ClrType))
                     .SelectMany(e => e.GetForeignKeys()))
        {
            fk.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}