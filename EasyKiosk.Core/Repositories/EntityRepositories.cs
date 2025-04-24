using EasyKiosk.Core.Model.Entities;

namespace EasyKiosk.Core.Repositories;

public interface IProductRepository : ICrudRepository<Product> {}
public interface ICategoryRepository : ICrudRepository<Category> {}
public interface IDeviceRepository : ICrudRepository<Device>{}