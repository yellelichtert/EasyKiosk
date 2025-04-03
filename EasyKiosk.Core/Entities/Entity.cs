using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Entity
{
    [Required]
    [Key]
    public int Id;
    
    
    [Required]
    public DateTime CreatedAt;

    [Required]
    public DateTime UpdatedAt;
}