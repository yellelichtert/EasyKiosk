using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model;

internal class Order : TrackedEntity
{
    public OrderState State { get; set; } = OrderState.InProgress;
    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    public Guid DeviceId { get; set; }
    

    public string OrderNumber => Id.ToString().Substring(Id.ToString().Length - 3);



    public void UpdateState()
    {
        if (State == OrderState.InProgress)
        {
            State = OrderState.Ready;
        }

        else if (State == OrderState.Ready)
        {
            State = OrderState.Finished;
        }
    }
    
}



