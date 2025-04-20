using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace EasyKiosk.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private IDbContextFactory<EasyKioskDbContext> _dbFactory;


    public ProductRepository(IDbContextFactory<EasyKioskDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }


    public Product[] GetAll()
    {
        using (var db  = _dbFactory.CreateDbContext())
        {
            return db.Products.ToArray();
        }
    }
    


    public async Task<Product?> GetByIdAsync(Guid id)
    {
        using (var db  = await _dbFactory.CreateDbContextAsync())
        {
            return await db.Products.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
 

    
    
    public async Task AddAsync(Product entity)
    {
        using (var db  = await _dbFactory.CreateDbContextAsync())
        {
            await db.Products.AddAsync(entity);
            await db.SaveChangesAsync();
        }
    }
        
    
    

    public async Task UpdateAsync(Product entity)
    {
        using (var db  = await _dbFactory.CreateDbContextAsync())
        {
            db.Products.Update(entity);
            await db.SaveChangesAsync();
        }
       
    }

    
    
    
    
    public async Task DeleteAsync(Product entity)
    {
        using (var db = await _dbFactory.CreateDbContextAsync())
        {
            db.Products.Remove(entity);
            await db.SaveChangesAsync();
        }
    }
}