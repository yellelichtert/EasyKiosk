using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model.Responses;

public sealed record UpdateOrderResponse
{
    public string orderNumber { get; init; }
    public OrderState State { get; init; }
}