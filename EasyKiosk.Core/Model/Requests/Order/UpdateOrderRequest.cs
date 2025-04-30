namespace EasyKiosk.Core.Model.Requests.Order;

public sealed record UpdateOrderRequest
{
    public required string orderNumber { get; init; }
}