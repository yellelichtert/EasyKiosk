

namespace EasyKiosk.Core.Entities;

public class Category : Entity
{
    public string Name;

    public string? Img;

    public List<Product> Products = new();
}