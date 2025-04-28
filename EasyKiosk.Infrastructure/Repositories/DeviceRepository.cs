using EasyKiosk.Core.Context;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Repositories;
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
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            await db.Devices.AddAsync(entity);
            await db.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(Device entity)
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            db.Devices.Update(entity);
            await db.SaveChangesAsync();
        }
    }

    public Task DeleteAsync(Device entity)
    {
        throw new NotImplementedException();
    }
}