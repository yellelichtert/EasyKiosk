using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private EasyKioskDbContext _context;
    private IDbContextFactory<EasyKioskDbContext> _dbFactory;
    

    public CategoryRepository(EasyKioskDbContext context)
    {
        _context = context;
    }


    public Category[] GetAll()
        => _context.Categories.ToArray();
    


    public Category? GetById(Guid id)
        => _context.Categories.FirstOrDefault(c => c.Id == id);
    
    
    public Task AddAsync(Category entity)
    {
        throw new NotImplementedException();
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