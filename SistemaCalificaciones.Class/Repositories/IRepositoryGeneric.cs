using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public interface IRepositoryGeneric<T> where T : IEntity
{
    Task<int> Create(T entity);
    Task Delete(int id);
    Task<ICollection<T>> GetAll();
    Task<T?> GetById(int id);
    Task Update(T entity);
}