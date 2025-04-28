namespace EasyKiosk.Core.Model.Requests;

public class OrderRequest
{
    public required Dictionary<Guid, int> Order { get; init; }
    public required Guid DeviceId { get; init; }
}