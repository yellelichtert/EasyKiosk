using EasyKiosk.Core.Entities;

namespace EasyKiosk.Core.Repositories;

public interface IProductRepository : ICrudRepository<Product> {}
public interface ICategoryRepository : ICrudRepository<Category> {}
public interface IUserRepository : ICrudRepository<User> {}