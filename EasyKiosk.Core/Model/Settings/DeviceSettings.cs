using DeviceType = EasyKiosk.Core.Model.Enums.DeviceType;

namespace EasyKiosk.Core.Model.Settings;

public class DeviceSettings
{
    public bool IsInitialSetup { get; set; }
    public string? ServerAdress { get; set; }
    public int? Port { get; set; }
    public Guid? DeviceId { get; set; }
    public DeviceType? DeviceType { get; set; }
    public string? AccesKey { get; set; }
    public string? RefreshKey { get; set; }

    public string FullAdress => $"http://{ServerAdress}:{Port}";

}