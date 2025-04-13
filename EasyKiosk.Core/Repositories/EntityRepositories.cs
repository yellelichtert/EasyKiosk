using EasyKiosk.Core.Entities;

namespace EasyKiosk.Core.Repositories;

public interface IProductRepository : ICrudRepository<Product> {}
public interface ICategoryRepository : ICrudRepository<Category> {}
public interface IStaffMemberRepository : ICrudRepository<StaffMember> {}
public interface IDeviceRepository : ICrudRepository<Device>{}