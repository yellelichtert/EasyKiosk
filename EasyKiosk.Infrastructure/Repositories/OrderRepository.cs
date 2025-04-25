using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace EasyKiosk.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private IDbContextFactory<EasyKioskDbContext> _contextFactory;

    
    public OrderRepository(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    
    public Task<Order?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Order[] GetAll()
    {
        using (var db = _contextFactory.CreateDbContext())
        {
            return db.Orders.ToArray();
        }
    }

    public async Task AddAsync(Order entity)
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            await db.Orders.AddAsync(entity);
            await db.SaveChangesAsync();
        }
    }

    public Task UpdateAsync(Order entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Order entity)
    {
        throw new NotImplementedException();
    }
}