using System.Text.Json.Serialization;

namespace EasyKiosk.Core.Model.DTO;

public sealed record OrderDetailDto
{
    public required Guid ProductId { get; init; }
    public required string ProductName { get; init; }
    public  required int Qty { get; init; }
    
    [JsonConstructor]
    internal  OrderDetailDto() { }
}


internal static partial class Mappers
{
    internal static OrderDetailDto MapToDto(this OrderDetail detail)
    {
        return new OrderDetailDto()
        {
            ProductId = detail.ProductId,
            ProductName = detail.Product.Name,
            Qty = detail.Qty,
        };
    }

}