using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public interface IDocenteRepository : IRepositoryGeneric<DocenteModel>
{
    Task<DocenteModel?> GetByUsuario(string usuario);
}