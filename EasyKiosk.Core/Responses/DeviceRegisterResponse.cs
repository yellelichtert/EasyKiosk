using EasyKiosk.Core.Enums;

namespace EasyKiosk.Core.Responses;

public record DeviceRegisterResponse(
    Guid Id,
    string Key
    );