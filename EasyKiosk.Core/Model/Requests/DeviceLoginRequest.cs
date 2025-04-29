namespace EasyKiosk.Core.Model.Requests;

public sealed record DeviceLoginRequest
{
    public required Guid Id { get; init; }
    public required string Key { get; init; }
};