using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private IDbContextFactory<EasyKioskDbContext> _contextFactory;

    public DeviceRepository(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    
    public Device? GetById(Guid id)
    {
        using (var db = _contextFactory.CreateDbContext())
        {
            return db.Devices.FirstOrDefault(x => x.Id == id);
        }
    }

    public Device[] GetAll()
    {
        using (var db = _contextFactory.CreateDbContext())
        {
            return db.Devices.ToArray();
        }
    }

    public Task AddAsync(Device entity)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Device entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Device entity)
    {
        throw new NotImplementedException();
    }
}