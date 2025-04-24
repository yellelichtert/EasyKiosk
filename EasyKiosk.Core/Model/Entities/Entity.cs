namespace EasyKiosk.Core.Model.Entities;

public class Entity
{
    public Guid Id { get; set; }
}

public class TrackedEntity : Entity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}