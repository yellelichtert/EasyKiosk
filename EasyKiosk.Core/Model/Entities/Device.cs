using EasyKiosk.Core.Model.Enums;

namespace EasyKiosk.Core.Model.Entities;

public class Device : TrackedEntity
{
    public string Name { get; set; }
    public DeviceType DeviceType { get; set; }
    public string Key { get; set; }
    public bool IsKeyRevoked { get; set; } = false;
}