
namespace EasyKiosk.Core.Model;

internal class OrderDetail : Entity
{
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    
    public int Qty { get; set; } = 1;
    public decimal PayedPrice { get; init; }

    public Product Product { get; init; }

    public OrderDetail(){}
    internal OrderDetail(Product product)
    {
        ProductId = product.Id;
        PayedPrice = product.Price;
        Product = product;
    }
    
}