using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IMenuService
{
    Task<IReadOnlyList<Product>> GetProductsAsync();
    Task<ErrorOr<Product>> AddProductAsync(Product product);
    Task<ErrorOr<Product>> UpdateProductAsync(Product product);
    Task<ErrorOr<bool>> DeleteProductAsync(Product product);
    

    Task<IReadOnlyList<Category>> GetCategoriesAsync();
    Task<ErrorOr<Category>> AddCategoryAsync(Category category);
    Task<ErrorOr<Category>> UpdateCategoryAsync(Category category);
    Task<ErrorOr<bool>> DeleteCategoryAsync(Category category);
}