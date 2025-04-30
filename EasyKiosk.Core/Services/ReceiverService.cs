using EasyKiosk.Core.Context;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Requests.Order;
using EasyKiosk.Core.Model.Responses;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Core.Services;

public class ReceiverService : IReceiverService
{
    private IDbContextFactory<EasyKioskDbContext> _contextFactory;

    public ReceiverService(IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }
    
    
    public async Task<OrderDto[]> GetOpenOrdersAsync()
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            return db.Orders
                .Where(o => o.State != OrderState.Finished)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .Select(o => o.MapToDto())
                .ToArray();
        }
    }

    
    
    public async Task<ErrorOr<OrderDto>> PlaceOrderAsync(OrderRequest request)
    {
        if (!request.Order.Any())
        {
            return Error.Validation("Order Cannot be Empty!");
        }

        
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            var order = new Order()
            {
                DeviceId = request.DeviceId,
                OrderDetails = request.Order.Select(d => new OrderDetail()
                {
                    ProductId = d.Key,
                    Qty = d.Value,
                    PayedPrice = db.Products.First(p => p.Id == d.Key).Price
                }).ToArray()
            };

            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            return order.MapToDto();
        }
    }

    
    public async Task<ErrorOr<UpdateOrderResponse>> UpdateOrderStateAsync(UpdateOrderRequest request)
    {
        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            var openOrders = await db.Orders.Where(o => o.State != OrderState.Finished).ToArrayAsync();
            var order = openOrders.FirstOrDefault(o => o.OrderNumber == request.orderNumber);
            
            
            if (order is not null)
            {
                order.UpdateState();
                
                db.Orders.Update(order);
                await db.SaveChangesAsync();

                return new UpdateOrderResponse()
                {
                    orderNumber = order.OrderNumber,
                    State = order.State
                };
            }
        }

        return Error.NotFound();
    }
}