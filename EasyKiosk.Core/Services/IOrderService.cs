using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IOrderService
{
    Task<ErrorOr<Order>> PlaceOrderAsync(Order order);
    Order[] GetOpenOrders();

}