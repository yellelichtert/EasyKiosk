using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Category : Entity
{
    [Required]
    public string Name;
    
    [Required]
    public List<Product> Products;
    
}