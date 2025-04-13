using EasyKiosk.Core.Entities;

namespace EasyKiosk.Core.Services;

public interface IDeviceService
{
    Device[] GetAllDevices();   
    Device? GetById(Guid id);

    Task<bool> ValidateKeyAsync(Guid id, string key);

}