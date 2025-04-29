using System.Text.Json.Serialization;
using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model.DTO;

public sealed record OrderDto
{
    public required string OrderNumber { get; init; }
    public required OrderDetailDto[] OrderDetails { get; init; }
    public required OrderState State { get; set; }
    public required DateTime Time { get; init; }

    [JsonConstructor]
    internal OrderDto() { }
}



internal static partial class Mappers
{
    internal static OrderDto MapToDto(this Order order)
    {
        return new OrderDto()
        {
            OrderNumber = order.OrderNumber,
            OrderDetails = order.OrderDetails.Select(od => od.MapToDto()).ToArray(),
            State = order.State,
            Time = order.CreatedAt
        };
    }
}