using EasyKiosk.Core.Model.Entities;

namespace EasyKiosk.Core.Model.DTO;

public sealed record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Img { get; init; }
}


internal static partial class Mappers
{
    internal static CategoryDto MapToDto(this Category category)
    {
        return new CategoryDto()
        {
            Id = category.Id,
            Name = category.Name,
            Img = category.Img
        };
    }
}