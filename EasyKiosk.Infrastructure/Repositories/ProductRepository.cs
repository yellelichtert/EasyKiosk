using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace EasyKiosk.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private EasyKioskDbContext _context;
    // private IDbContextFactory<EasyKioskDbContext> _dbFactory;
    // private DbSet<Product>? _entities;

    public ProductRepository(EasyKioskDbContext context)
    {
        _context = context;
    }


    public Product[] GetAll()
        => _context.Products.ToArray();


    public Product? GetById(Guid id)
        => _context.Products.FirstOrDefault(p => p.Id == id);

    
    
    
    public async Task AddAsync(Product entity)
    {
        await _context.Products.AddAsync(entity);
        await _context.SaveChangesAsync();
    }
        
    
    

    public async Task UpdateAsync(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    
    
    
    
    public async Task DeleteAsync(Product entity)
    {
        _context.Products.Remove(entity);
        await _context.SaveChangesAsync();
    }
}