using EasyKiosk.Core.Enums;

namespace EasyKiosk.Core.Responses;

public record DeviceLoginResponse(
    string Token,
    DeviceType Type
    );