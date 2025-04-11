namespace EasyKiosk.Core.Entities;

public class Order : Entity
{
    public List<Product> Products { get; set; } = new();
}