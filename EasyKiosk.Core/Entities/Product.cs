using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Product : Entity
{
    [Required]
    public string Name;
    
    [Required]
    public string Description;
    
    [Required]
    public decimal Price;
    
    [Required]
    public Category Category;
}