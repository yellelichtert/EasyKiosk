using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model.Responses;

public sealed record DeviceRegisterResponse
{
    public Guid Id { get; init; }
    public DeviceType Type { get; init; }
    public string Token { get; init; }
    public string Refresh { get; init; }
}
   
    