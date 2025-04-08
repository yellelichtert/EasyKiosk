using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private IDbContextFactory<EasyKioskDbContext> _dbFactory;
    

    public CategoryRepository(IDbContextFactory<EasyKioskDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }


    public Category[] GetAll()
    {
        using (var db = _dbFactory.CreateDbContext())
        {
            return db.Categories.Include(c => c.Products).ToArray();
        }
    }
        
    


    public Category? GetById(Guid id)
    {
        using (var db = _dbFactory.CreateDbContext())
        {
            return db.Categories.FirstOrDefault(c => c.Id == id);
        }
    }

    
    

    public async Task AddAsync(Category entity)
    {
        using (var db = await _dbFactory.CreateDbContextAsync())
        {
            await db.Categories.AddAsync(entity);
            await db.SaveChangesAsync();
        }
    }



    public async Task UpdateAsync(Category entity)
    { 
        using (var db = await _dbFactory.CreateDbContextAsync())
        {
            db.Categories.Update(entity);
            await db.SaveChangesAsync();
        }
        
    }
        

    
    public async Task DeleteAsync(Category entity)
    {
        using (var db = await _dbFactory.CreateDbContextAsync())
        {
            db.Categories.Remove(entity);
            await db.SaveChangesAsync();
        }
    }
    
}