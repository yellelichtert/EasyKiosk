using EasyKiosk.Core.Model.Entities;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IMenuService
{
    IEnumerable<Product> GetProducts();
    Task<ErrorOr<Product>> AddProductAsync(Product product);
    Task<ErrorOr<Product>> UpdateProductAsync(Product product);
    Task<ErrorOr<bool>> DeleteProductAsync(Product product);
    

    IEnumerable<Category> GetCategories();
    Task<string> GetCategoryNameAsync(Guid id);
    Task<ErrorOr<Category>> AddCategoryAsync(Category category);
    Task<ErrorOr<Category>> UpdateCategoryAsync(Category category);
    Task<ErrorOr<bool>> DeleteCategoryAsync(Category category);
}