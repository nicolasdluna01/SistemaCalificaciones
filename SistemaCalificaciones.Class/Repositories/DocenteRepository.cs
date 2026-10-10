using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public class DocenteRepository : RepositoryGeneric<DocenteModel>, IDocenteRepository
{
    public DocenteRepository(SistemaCalificacionesDB db) : base(db) { }

    public async Task<DocenteModel?> GetByUsuario(string usuario)
    {
        return await dbSet.FirstOrDefaultAsync(d => d.Usuario == usuario);
    }
}