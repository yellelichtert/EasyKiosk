using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model.Responses;

public record DeviceRegisterResponse(
    Guid Id,
    DeviceType Type,
    string Token,
    string Refresh
    );