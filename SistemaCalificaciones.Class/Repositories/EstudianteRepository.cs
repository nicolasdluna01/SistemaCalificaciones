using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public class EstudianteRepository : RepositoryGeneric<EstudianteModel>, IEstudianteRepository
{
    public EstudianteRepository(SistemaCalificacionesDB db) : base(db) { }

    public async Task<EstudianteModel?> GetByMatricula(string matricula)
    {
        return await dbSet.FirstOrDefaultAsync(e => e.Matricula == matricula);
    }
}