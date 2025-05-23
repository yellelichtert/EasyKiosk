using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Requests.Order;
using EasyKiosk.Core.Model.Responses;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IReceiverService
{
    Task<OrderDto[]> GetOpenOrdersAsync();
    Task<ErrorOr<OrderDto>> PlaceOrderAsync(OrderRequest request);

    Task<ErrorOr<UpdateOrderResponse>> UpdateOrderStateAsync(UpdateOrderRequest request);
}