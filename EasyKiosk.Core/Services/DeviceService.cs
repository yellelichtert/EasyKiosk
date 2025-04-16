using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Core.Requests;
using EasyKiosk.Core.Utils;
using ErrorOr;

namespace EasyKiosk.Core.Services;


public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;


    public DeviceService(IDeviceRepository repository)
    {
        _repository = repository;
    }
    
    
    public Device[] GetAllDevices()
        => _repository.GetAll();



    public async Task<ErrorOr<Device>> ValidateLoginAsync(DeviceLoginRequest request)
    {
        var device = await _repository.GetByIdAsync(request.Id);
        
        if (device == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Key, device.Key))
        {
            return Error.Unauthorized();
        }

        return device;
    }

    public async Task<ErrorOr<(Device, string)>> AddDeviceAsync(Device device)
    {
        var key = KeyGenerator.GenerateKey(50);
        device.Key = BCrypt.Net.BCrypt.EnhancedHashPassword(key);
        
        if (!ValidateDevice(device, out var errors))
            return errors;

        await _repository.AddAsync(device);
        return (device, key);
    }



    private bool ValidateDevice(Device device, out List<Error> errors)
    {
        errors = new();

        var DeviceWithSameName = _repository.GetAll().FirstOrDefault(d => d.Name.ToLower() == device.Name.ToLower());
        if (DeviceWithSameName != null && DeviceWithSameName.Id != device.Id)
        {
            errors.Add(Error.Conflict("Name must be unique!"));
        }

        if (string.IsNullOrWhiteSpace(device.Key))
        {
            errors.Add(Error.Failure("Api key failed to generate..."));
        }

        if (device.DeviceType.Equals(null))
        {
            errors.Add(Error.Validation("Please select a deviceType!"));
        }

        return !errors.Any();
    }

    // public Task<bool> ValidateKeyAsync(Guid id, string key)
    // {
    //     var dbResult = _repository.GetById(id);
    //
    //     if (dbResult is null || dbResult.IsKeyRevoked || dbResult.Key != key) //todo: Move to checkPassword with hasher.
    //     {
    //         return Task.FromResult(false);
    //     }
    //
    //     return Task.FromResult(true);
    // }
}