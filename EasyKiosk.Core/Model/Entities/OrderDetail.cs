using EasyKiosk.Core.Model.Entities;

namespace EasyKiosk.Core.Model;

public class OrderDetail : Entity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    
    public int Qty { get; set; } = 1;
    public decimal PayedPrice { get; }

    public required Product Product { get; set; }
}