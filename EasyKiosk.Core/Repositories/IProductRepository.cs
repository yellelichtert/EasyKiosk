using EasyKiosk.Core.Entities;

namespace EasyKiosk.Core.Repositories;

public interface IProductRepository : ICrudRepository<Product>
{
    Task DeleteAsync(IEnumerable<Product> entities);

}