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
    
    
    public async Task<Device?> GetByIdAsync(Guid id)
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            return await db.Devices.FirstOrDefaultAsync(x => x.Id == id);
        }
    }

    public Device[] GetAll()
    {
        using (var db = _contextFactory.CreateDbContext())
        {
            return db.Devices.ToArray();
        }
    }

    public async Task AddAsync(Device entity)
    {
        using (var db = _contextFactory.CreateDbContext())
        {
            await db.Devices.AddAsync(entity);
            await db.SaveChangesAsync();
        }
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