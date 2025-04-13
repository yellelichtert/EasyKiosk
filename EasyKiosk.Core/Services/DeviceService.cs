using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;

namespace EasyKiosk.Core.Services;

public class DeviceService : IDeviceService
{
    private IDeviceRepository _repository;


    public DeviceService(IDeviceRepository repository)
    {
        _repository = repository;
    }
    
    
    public Device[] GetAllDevices()
        => _repository.GetAll();

    public Device? GetById(Guid id)
        => _repository.GetById(id);

    public Task<bool> ValidateKeyAsync(Guid id, string key)
    {
        var dbResult = _repository.GetById(id);

        if (dbResult is null || dbResult.IsKeyRevoked || dbResult.Key != key) //todo: Move to checkPassword with hasher.
        {
            return Task.FromResult(false);
        }

        return Task.FromResult(true);
    }
}