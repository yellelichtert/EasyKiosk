using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Model.Responses.Device;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IDeviceService
{
    Task<IReadOnlyList<Device>> GetDevicesAsync();   

    Task<ErrorOr<DeviceLoginResponse>> LoginAsync(DeviceLoginRequest request);
    
    Task<ErrorOr<DeviceRegisterResponse>> AddDeviceAsync(Device device);

}