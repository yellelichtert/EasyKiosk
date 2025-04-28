using EasyKiosk.Core.Model.DTO;

namespace EasyKiosk.Core.Model.Responses;

public sealed record OrderResponse
{
    public required string OrderNumber { get; init; }
    public required DateTime Time { get; init; }
}


public static partial class Mappers
{
    public static OrderResponse MapToResponse(this OrderDto order)
    {
        return new OrderResponse()
        {
            OrderNumber = order.OrderNumber,
            Time = order.Time
        };
    }
}