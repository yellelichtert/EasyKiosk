namespace EasyKiosk.Core.Requests;

public record DeviceLoginRequest(
    Guid Id,
    string Key
    );