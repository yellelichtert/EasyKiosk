namespace EasyKiosk.Core.Entities;

public class Product : Entity
{
    public string Name;
    
    public string Description;
    
    public decimal Price;

    public string? Img;
    
    public Category Category;
    
    public Guid CategoryId;
}