using EasyKiosk.Core.Enums;

namespace EasyKiosk.Core.Entities;

public class Device : Entity
{
    public string Name { get; set; }
    public DeviceType DeviceType { get; set; }
    public string Key { get; set; }
    public bool IsKeyRevoked { get; set; } = false;
}