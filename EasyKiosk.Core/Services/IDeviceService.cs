using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Requests;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IDeviceService
{
    Device[] GetAllDevices();   

    Task<ErrorOr<Device>> ValidateLoginAsync(DeviceLoginRequest request);
    
    Task<ErrorOr<(Device, string)>> AddDeviceAsync(Device device);

}