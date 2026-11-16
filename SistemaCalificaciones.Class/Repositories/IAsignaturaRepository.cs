using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public interface IAsignaturaRepository : IRepositoryGeneric<AsignaturaModel>
{
    Task<bool> ExisteClave(string clave, int excluirId = 0);
}