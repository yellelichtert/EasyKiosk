using DeviceType = EasyKiosk.Core.Model.Enums.DeviceType;

namespace EasyKiosk.Core.Model.Options;

public class DeviceOptions
{
    public bool InitialSetup { get; set; }
    public string ServerAdress { get; set; }
    public Guid DeviceId { get; set; }
    public DeviceType DeviceType { get; set; }
}