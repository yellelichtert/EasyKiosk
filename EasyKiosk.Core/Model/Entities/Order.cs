namespace EasyKiosk.Core.Model.Entities;

public class Order : TrackedEntity
{
    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
    public Guid DeviceId { get; set; }
}



