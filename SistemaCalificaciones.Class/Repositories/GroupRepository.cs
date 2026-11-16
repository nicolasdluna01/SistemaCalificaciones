using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public class GrupoRepository : RepositoryGeneric<GrupoModel>, IGrupoRepository
{
    public GrupoRepository(SistemaCalificacionesDB db) : base(db) { }

    public async Task<bool> ExisteGrupo(string nombreGrupo, string periodo, int excluirId = 0)
    {
        return await dbSet.AnyAsync(g =>
            g.NombreGrupo == nombreGrupo && g.Periodo == periodo && g.Id != excluirId);
    }
}