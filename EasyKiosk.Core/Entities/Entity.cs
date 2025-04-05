using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Entity
{
    [Required]
    [Key]
    public Guid Id;
    
    
    [Required]
    public DateTime CreatedAt;

    [Required]
    public DateTime UpdatedAt;
}