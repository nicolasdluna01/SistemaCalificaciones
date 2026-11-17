using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public interface IEstudianteRepository : IRepositoryGeneric<EstudianteModel>
{
    Task<EstudianteModel?> GetByMatricula(string matricula);
}