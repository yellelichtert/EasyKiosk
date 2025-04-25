using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model;

public class Order : TrackedEntity
{
    public OrderState State = OrderState.InProgress;
    public ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
    public Guid DeviceId { get; set; }
    

    public string OrderNumber => Id.ToString().Substring(Id.ToString().Length - 3);



    public void AddProduct(Product product)
    {
        var orderDetail = GetOrderDetail(product);

        if (orderDetail is null)
        {
            OrderDetails.Add(new OrderDetail(product));
        }
        else
        {
            orderDetail.Qty++;
        }
    }
    
    
    public void RemoveProduct(Product product)
    {
        var orderDetail = GetOrderDetail(product);
        
        if (orderDetail is null)
        {
            return;
        }

        orderDetail.Qty--;

        if (orderDetail.Qty < 1)
        {
            OrderDetails.Remove(orderDetail);
        }

    }


    public OrderDetail? GetOrderDetail(Product product)
        => OrderDetails.FirstOrDefault(x => x.Product == product);
}



