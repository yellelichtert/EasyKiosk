using EasyKiosk.Core.Context;
using EasyKiosk.Core.Model.DTO;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Core.Services;

public class KioskService : IKioskService
{
    private IDbContextFactory<EasyKioskDbContext> _contextFactory;

    public KioskService(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    
    
    public async Task<(CategoryDto[] categories, ProductDto[] products)> GetMenuDataAsync()
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            var categories = db.Categories.Select(c => c.MapToDto()).ToList();
            
            categories.Insert(0, new CategoryDto()
            {
                Id = Guid.Empty,
                Name = "All"
            });
            

            var products = db.Products.Select(p => p.MapToDto()).ToArray();


            return (categories.ToArray(), products);
        }
    }
}