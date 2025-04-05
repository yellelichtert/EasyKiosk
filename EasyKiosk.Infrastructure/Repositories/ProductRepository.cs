using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace EasyKiosk.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private IDbContextFactory<EasyKioskDbContext> _dbFactory;
    private DbSet<Product>? _entities;

    public ProductRepository(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _dbFactory = contextFactory;
    }


    public Product[] GetAll()
    {
        if (_entities is not null)
            return _entities.ToArray();
        
        
        using (var context = _dbFactory.CreateDbContext())
        {
            return context.Products.ToArray();
        }
    }
        
    
    public  Product GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    
    
    
    public async Task AddAsync(Product entity)
    {
        using (var context = await _dbFactory.CreateDbContextAsync())
        {
            await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
        }
        
        _entities = null;
    }
        
    
    

    public async Task UpdateAsync(Product entity)
    {
        using (var context = await _dbFactory.CreateDbContextAsync())
        {
            context.Products.Update(entity);
            await context.SaveChangesAsync();
        }
        
        _entities = null;
    }

    
    
    
    
    public async Task DeleteAsync(Product entity)
    {
        using (var context = await _dbFactory.CreateDbContextAsync())
        {
            context.Products.Remove(entity);
            await context.SaveChangesAsync();
        }
        
        _entities = null;
    }


    
    
    public async Task DeleteAsync(IEnumerable<Product> entities)
    {
        using (var context = await _dbFactory.CreateDbContextAsync())
        {
            context.Products.RemoveRange(entities);
            await context.SaveChangesAsync();
        }


        _entities = null;
    }
}