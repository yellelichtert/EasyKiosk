using System.Collections.ObjectModel;
using EasyKiosk.Core.Context;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Repositories;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Core.Services;

public class MenuService : IMenuService
{
    
    private readonly IDbContextFactory<EasyKioskDbContext> _contextFactory;


    
    
    public MenuService(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        
        _contextFactory = contextFactory;
        
    }
    
    
    
    
    //
    // Product methods
    //
    
    
    
    
    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {

        using (var db = await _contextFactory.CreateDbContextAsync())
        {

            return db.Products.ToArray();

        }
        
    }




    public async Task<ErrorOr<Product>> AddProductAsync(Product product)
    {

        var validationResult = await ValidateProductAsync(product);


        if (validationResult.Any())
        {

            return validationResult;

        }


        using (var db = await _contextFactory.CreateDbContextAsync())
        {

            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();

        }
        
        
        return product;
    }
    

    
    
    public async Task<ErrorOr<Product>> UpdateProductAsync(Product product)
    {

        var validationResult = await ValidateProductAsync(product);


        if (validationResult.Any())
        {

            return validationResult;
            
        }


        await using (var db = await _contextFactory.CreateDbContextAsync())
        {

            db.Update(product);
            await db.SaveChangesAsync();

        }

        
        return product;
        
    }

    
    
    
    public async Task<ErrorOr<bool>> DeleteProductAsync(Product product)
    {

        using (var db = await _contextFactory.CreateDbContextAsync())
        {

            var dbResult = await db.Products.FirstOrDefaultAsync(p => p.Id == product.Id);

            
            if (dbResult is null)
            {
                
                return Error.NotFound(description: "Product not Found!");

            }


            db.Remove(product);
            await db.SaveChangesAsync();
            
        }
        

        return true;
        
    }

    
    
    
    private async Task<List<Error>> ValidateProductAsync(Product product)
    {
        
        var errors = new List<Error>();


        await using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            var productWithSameName = db.Products.FirstOrDefault(p => p.Name == product.Name);
            
            
            if (productWithSameName != null && productWithSameName.Id != product.Id)
            {
                
                errors.Add(Error.Conflict(description: "Name must be unique!"));
                
            }
            
        }
        
    
        if (product.Price < 0)
        {
            
            errors.Add(Error.Validation(description: "Price cannot be negative!"));
            
        }
        
        
        if (product.CategoryId == Guid.Empty)
        {
            
            errors.Add(Error.NotFound(description: "Category cannot be null!"));
            
        }

        
        return errors;
    }

    
    
    
    //
    // Category methods
    //

    
    
    
    public async Task<IReadOnlyList<Category>> GetCategoriesAsync()
    {
        
        //todo: Add Filters
        
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            return db.Categories.ToArray();
            
        }
        
    }
    
    
    
    
    public async Task<ErrorOr<Category>> AddCategoryAsync(Category category)
    {

        var validationResult = await ValidateCategoryAsync(category);


        if (validationResult.Any())
        {

            return validationResult;

        }
        
        
        await using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            await db.Categories.AddAsync(category);
            await db.SaveChangesAsync();
            
        }
        
        
        return category;
        
    }

    
    

    public async Task<ErrorOr<Category>> UpdateCategoryAsync(Category category)
    {
        
        var validationResult = await ValidateCategoryAsync(category);
        
        
        if (validationResult.Any())
        {
            return validationResult;
        }
        

        await using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            db.Categories.Update(category);
            await db.SaveChangesAsync();
            
        }
        
        
        return category;
        
    }
    
    
    
    
    public async Task<ErrorOr<bool>> DeleteCategoryAsync(Category category) //todo: Replace with Id parameter.
    {
        
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            var dbResult = await db.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);


            if (dbResult is null)
            {
                
                return Error.NotFound(description: "Category not found!");
                
            }
                
            
            db.Categories.Remove(dbResult);
            await db.SaveChangesAsync();
            
        }
        
        
        return true;
        
    }

    
    
    
    private async Task<List<Error>> ValidateCategoryAsync(Category category) //todo: Replace with Id prarmater.
    {
        
        var errors = new List<Error>();
        
        
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            var categoryWithSameName = db.Categories.FirstOrDefault(c => c.Name == category.Name);
            
            
            if (categoryWithSameName != null && categoryWithSameName.Id != category.Id)
            {
                
                errors.Add(Error.Conflict(description: "Name must be unique!"));
                
            }
            
        }


        return errors;

    }
    
}