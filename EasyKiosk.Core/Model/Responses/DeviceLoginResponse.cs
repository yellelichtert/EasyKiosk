namespace EasyKiosk.Core.Model.Responses;

public record DeviceLoginResponse(
    string Token,
    string Refresh
    );