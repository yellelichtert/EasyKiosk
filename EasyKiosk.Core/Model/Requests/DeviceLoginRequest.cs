namespace EasyKiosk.Core.Model.Requests;

public record DeviceLoginRequest(
    Guid Id,
    string Key
    );