using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using ErrorOr;

namespace EasyKiosk.Core.Services;

public interface IDeviceService
{
    Device[] GetAllDevices();   

    Task<ErrorOr<DeviceLoginResponse>> LoginAsync(DeviceLoginRequest request);
    
    Task<ErrorOr<DeviceRegisterResponse>> AddDeviceAsync(Device device);

}