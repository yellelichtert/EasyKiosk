

namespace EasyKiosk.Core.Model;

public class Category : TrackedEntity
{
    public string Name { get; set; }

    public string? Img { get; set; }

    public List<Product> Products { get; set; } = new();
}