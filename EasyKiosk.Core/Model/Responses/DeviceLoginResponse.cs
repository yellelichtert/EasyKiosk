namespace EasyKiosk.Core.Model.Responses;

public sealed record DeviceLoginResponse
{
    public required string Token { get; init; }
    public required string Refresh { get; init; }
}