namespace EasyKiosk.Core.Model.DTO;

public sealed record ProductDto
{
    public required Guid Id { get; init; }
    public required Guid CategoryId { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required decimal Price { get; init; }
    public string? Img { get; init; }
    
    internal ProductDto(){}
}


internal static partial class Mappers
{
    internal static ProductDto MapToDto(this Product product)
    {
        return new ProductDto()
        {
            Id = product.Id,
            CategoryId = product.CategoryId,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Img = product.Img
        };
    }
}