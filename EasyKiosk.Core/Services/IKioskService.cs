using EasyKiosk.Core.Model.DTO;

namespace EasyKiosk.Core.Services;

public interface IKioskService
{
    Task<(CategoryDto[] categories, ProductDto[] products)> GetMenuDataAsync();
}