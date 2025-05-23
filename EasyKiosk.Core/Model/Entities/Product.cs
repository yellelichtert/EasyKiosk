
namespace EasyKiosk.Core.Model;

public class Product : TrackedEntity
{
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public decimal Price { get; set; }

    public string? Img { get; set; }

    public Category Category;
    
    public Guid CategoryId { get; set; }


    public override string ToString()
    {
        return Name;
    }
}