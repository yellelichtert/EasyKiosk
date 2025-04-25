using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Repositories;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public class OrderService : IOrderService
{
    private IOrderRepository _orders;

    public OrderService(IOrderRepository orders)
    {
        _orders = orders;
    }
    
    
    
    public async Task<ErrorOr<Order>> PlaceOrderAsync(Order order)
    {
        if (!ValidateOrder(order, out var errors))
            return errors;

        await _orders.AddAsync(order);
        return order;
    }

    private bool ValidateOrder(Order order, out List<Error> errors)
    {
        errors = new();
        if (order.OrderDetails.Count == 0)
        {
            errors.Add(Error.NotFound("Order is empty!"));
        }
        
        return !errors.Any();
    }

    public Order[] GetOpenOrders()
        => _orders.GetAll().Where(x => x.State == OrderState.InProgress).ToArray();
}