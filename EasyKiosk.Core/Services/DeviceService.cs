using EasyKiosk.Core.Factory;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Core.Utils;
using ErrorOr;

namespace EasyKiosk.Core.Services;


public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _repository;
    private readonly ITokenFactory<Device> _tokenFactory;

    public DeviceService(IDeviceRepository repository, ITokenFactory<Device> tokenFactory)
    {
        _repository = repository;
        _tokenFactory = tokenFactory;
    }
    
    
    public Device[] GetAllDevices()
        => _repository.GetAll();



    public async Task<ErrorOr<DeviceLoginResponse>> LoginAsync(DeviceLoginRequest request)
    {
        var device = await _repository.GetByIdAsync(request.Id);
        
        if (device == null)
        {
            return Error.NotFound();
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Key, device.Key))
        {
            return Error.Unauthorized();
        }

        var refreshToken = CreateRefreshKey(device);
        await _repository.UpdateAsync(device);
        
        return new DeviceLoginResponse()
        {
            Token = _tokenFactory.CreateToken(device),
            Refresh = refreshToken
        };
    }
    
    

    public async Task<ErrorOr<DeviceRegisterResponse>> AddDeviceAsync(Device device)
    {
        var refreshKey = CreateRefreshKey(device);
        if (!ValidateDevice(device, out var errors))
            return errors;

        
        await _repository.AddAsync(device);
        
        
        return new DeviceRegisterResponse()
        {
            Id = device.Id,
            Type = device.DeviceType,
            Token = _tokenFactory.CreateToken(device),
            Refresh = refreshKey
        };
    }


    private string CreateRefreshKey(Device device)
    {
        var key = KeyGenerator.GenerateKey(20);
        device.Key = BCrypt.Net.BCrypt.HashPassword(key);
        
        return key;
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