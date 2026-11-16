using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public interface IGrupoRepository : IRepositoryGeneric<GrupoModel>
{
    Task<bool> ExisteGrupo(string nombreGrupo, string periodo, int excluirId = 0);
}