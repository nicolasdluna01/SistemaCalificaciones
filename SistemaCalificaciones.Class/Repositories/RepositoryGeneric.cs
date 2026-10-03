using Microsoft.EntityFrameworkCore;
using SistemaCalificaciones.Class.Data;
using SistemaCalificaciones.Class.Models;

namespace SistemaCalificaciones.Class.Repositories;

public class RepositoryGeneric<T> : IRepositoryGeneric<T> where T : class, IEntity
{
    protected readonly SistemaCalificacionesDB db;
    protected readonly DbSet<T> dbSet;

    public RepositoryGeneric(SistemaCalificacionesDB db)
    {
        this.db = db;
        dbSet = db.Set<T>();
    }

    public virtual async Task<int> Create(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
        dbSet.Add(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public virtual async Task Delete(int id)
    {
        var e = await dbSet.FirstOrDefaultAsync(x => x.Id == id);
        if (e == null) return;
        dbSet.Remove(e);
        await db.SaveChangesAsync();
    }

    public virtual async Task<ICollection<T>> GetAll()
    {
        return await dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetById(int id)
    {
        return await dbSet.FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task Update(T entity)
    {
        if (db.Entry(entity).State == EntityState.Detached)
        {
            dbSet.Attach(entity);
        }
        dbSet.Update(entity);
        await db.SaveChangesAsync();
    }
}