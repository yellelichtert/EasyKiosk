using EasyKiosk.Core.Model.DTO;

namespace EasyKiosk.Core.Model.Responses;

public sealed record KioskDataResponse
{
    public required CategoryDto[] Categories { get; init; }
    public required ProductDto[] Products { get; init; }
}