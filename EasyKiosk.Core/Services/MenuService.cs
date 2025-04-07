using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public class MenuService : IMenuService
{
    private readonly ICategoryRepository _categories;
    private readonly IProductRepository _products;


    public MenuService(ICategoryRepository categories, IProductRepository products)
    {
        _categories = categories;
        _products = products;
    }
    
    
    
    
    
    //
    // Product methods
    //
    public IEnumerable<Product> GetProducts()
        => _products.GetAll();
    
    
    public async Task<ErrorOr<Product>> AddProductAsync(Product product)
    {
        if (!ValidateProduct(product, out var errors))
            return errors;

        await _products.AddAsync(product);
        return product;
    }
    

    public async Task<ErrorOr<Product>> UpdateProductAsync(Product product)
    {
        if (!ValidateProduct(product, out var errors))
            return errors;

        await _products.UpdateAsync(product);
        return product;
    }

    
    public async Task<ErrorOr<bool>> DeleteProductAsync(Product product)
    {
        if (_products.GetById(product.Id) is null)
            return Error.NotFound(description: "Product not Found!");

        await _products.DeleteAsync(product);
        return true;
    }

    
    private bool ValidateProduct(Product product, out List<Error> errors)
    {
        errors = new();

        var productWithSameName = _products.GetAll().FirstOrDefault(p => p.Name == product.Name);
        if (productWithSameName != null && productWithSameName.Id != product.Id)
        {
            errors.Add(Error.Conflict(description: "Name must be unique!"));
        }

        if (product.Price < 0)
        {
            errors.Add(Error.Validation(description: "Price cannot be negative!"));
        }
        
        if (product.CategoryId == Guid.Empty)
        {
            errors.Add(Error.NotFound(description: "Category cannot be null!"));
        }
        
        return !errors.Any();
    }

    
    
    //
    // Category methods
    //

    public IEnumerable<Category> GetCategories()
        => _categories.GetAll();

    public IEnumerable<Product> GetProductsInCategory(Guid id)
        => _products.GetAll().Where(p => p.CategoryId == id);
    
    public string GetCategoryName(Guid id)
        => _categories.GetById(id)!.Name;
    
    public async Task<ErrorOr<Category>> AddCategoryAsync(Category category)
    {
        if (!ValidateCategory(category, out var errors))
            return errors;
        

        await _categories.AddAsync(category);
        return category;
    }


    public async Task<ErrorOr<Category>> UpdateCategoryAsync(Category category)
    {
        if (!ValidateCategory(category, out var errors))
            return errors;

        await _categories.UpdateAsync(category);
        return category;
    }
    
    
    public async Task<ErrorOr<bool>> DeleteCategoryAsync(Category category)
    {
        if (_categories.GetById(category.Id) is null)
            return Error.NotFound("Category not found!");

        if (category.Products.Any())
            return Error.Conflict("Category cannot contain any products!");
        
        
        await _categories.DeleteAsync(category);
        return true;
    }

    
    private bool ValidateCategory(Category category, out List<Error> errors)
    {
        errors = new();

        var categoryWithSameName = _categories.GetAll().FirstOrDefault(c => c.Name == category.Name);
        if (categoryWithSameName != null && categoryWithSameName.Id != category.Id)
        {
            errors.Add(Error.Conflict(description: "Name must be unique!"));
        }

        if (category.Products is null || category.Products.Count == 0)
        {
            errors.Add(Error.NotFound(description: "Products cannot be empty!"));
        }

        return !errors.Any();
    }
    
}