using EasyKiosk.Core.Model.Entities;

namespace EasyKiosk.Core.Repositories;

public interface ICrudRepository<TEntity> where TEntity : Entity
{
    Task<TEntity?> GetByIdAsync(Guid id);
    TEntity[] GetAll();
    Task AddAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
}