using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private IDbContextFactory<EasyKioskDbContext> _dbFactory;


    public CategoryRepository(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _dbFactory = contextFactory;
    }


    public Category[] GetAll()
    {
        using (var context = _dbFactory.CreateDbContext())
        {
            return context.Categories.ToArray();
        }
    }



    public Category GetById(int id)
    {
        using (var context = _dbFactory.CreateDbContext())
        {
            return context.Categories.FirstOrDefault(c => c.Id == id);
        }
    }
    

    
    public Task AddAsync(Category entity)
    {
        Console.WriteLine("Adding Category");
        return Task.CompletedTask;
    }

    
    public Task UpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    
    public Task DeleteAsync(Category entity)
    {
        throw new NotImplementedException();
    }
}