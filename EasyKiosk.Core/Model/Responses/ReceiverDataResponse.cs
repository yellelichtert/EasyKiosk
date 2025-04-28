using EasyKiosk.Core.Model.DTO;

namespace EasyKiosk.Core.Model.Responses;

public sealed record ReceiverDataResponse
{
    public required OrderDto[] OpenOrders { get; init; }
}