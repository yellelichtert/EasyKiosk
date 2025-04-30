namespace EasyKiosk.Core.Model.Requests.Order;

public sealed record OrderRequest
{
    public required Dictionary<Guid, int> Order { get; init; }
    public required Guid DeviceId { get; init; }
}