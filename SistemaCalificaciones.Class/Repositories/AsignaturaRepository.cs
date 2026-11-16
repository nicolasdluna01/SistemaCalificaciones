using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public class AsignaturaRepository : RepositoryGeneric<AsignaturaModel>, IAsignaturaRepository
{
    public AsignaturaRepository(SistemaCalificacionesDB db) : base(db) { }

    public async Task<bool> ExisteClave(string clave, int excluirId = 0)
    {
        return await dbSet.AnyAsync(a => a.ClaveAsignatura == clave && a.Id != excluirId);
    }
}