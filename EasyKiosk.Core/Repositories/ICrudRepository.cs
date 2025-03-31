using EasyKiosk.Core.Entities;

namespace EasyKiosk.Core.Repositories;

public interface ICrudRepository<TEntity> where TEntity : Entity
{
    TEntity Get(int id);
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}