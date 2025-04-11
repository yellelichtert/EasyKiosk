using System.ComponentModel.DataAnnotations;

namespace EasyKiosk.Core.Entities;

public class Entity
{
    public Guid Id { get; set; }


    public DateTime CreatedAt;
     public DateTime UpdatedAt;
}