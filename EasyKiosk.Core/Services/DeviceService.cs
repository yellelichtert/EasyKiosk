using EasyKiosk.Core.Context;
using EasyKiosk.Core.Factory;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses.Device;
using EasyKiosk.Core.Utils;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace EasyKiosk.Core.Services;


public class DeviceService : IDeviceService
{
    
    private readonly IDbContextFactory<EasyKioskDbContext> _contextFactory;
    private readonly ITokenFactory<Device> _tokenFactory;

    
    
    
    public DeviceService(ITokenFactory<Device> tokenFactory, IDbContextFactory<EasyKioskDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
        _tokenFactory = tokenFactory;
    }


    
    
    public async Task<IReadOnlyList<Device>> GetDevicesAsync()
    {

        using (var db = await _contextFactory.CreateDbContextAsync())
        {

            return db.Devices.ToList();

        }
        
    }
  



    public async Task<ErrorOr<DeviceLoginResponse>> LoginAsync(DeviceLoginRequest request)
    {

        using (var db = await _contextFactory.CreateDbContextAsync())
        {
            
            var dbResult = await db.Devices.FirstOrDefaultAsync(d => d.Id == request.Id);


            if (dbResult is null)
            {

                return Error.NotFound();

            }


            if (!ValidateKey(dbResult, request.Key))
            {

                return Error.Unauthorized();

            }

            
            var refreshToken = CreateRefreshKey(dbResult);
            
            
            db.Update(dbResult);
            await db.SaveChangesAsync();


            return new DeviceLoginResponse()
            {
                Token = _tokenFactory.CreateToken(dbResult),
                Refresh = refreshToken
            };
            
        }
        
    }
    
    

    
    public async Task<ErrorOr<DeviceRegisterResponse>> AddDeviceAsync(Device device)
    {

        using (var db = await _contextFactory.CreateDbContextAsync())
        {

            var validationResult = await ValidateDevice(device, db);


            if (validationResult.Any())
            {
                return validationResult;
            }

            
            var refreshKey = CreateRefreshKey(device);


            await db.Devices.AddAsync(device);
            await db.SaveChangesAsync();
            
            
            return new DeviceRegisterResponse()
            {
                Id = device.Id,
                Type = device.DeviceType,
                Token = _tokenFactory.CreateToken(device),
                Refresh = refreshKey
            };
            
        }
        
    }

    
    

    private string CreateRefreshKey(Device device)
    {
        
        var key = KeyGenerator.GenerateKey(20);
        device.Key = BCrypt.Net.BCrypt.HashPassword(key);
        
        
        return key;
        
    }
    

    
    
    private async Task<List<Error>> ValidateDevice(Device device, EasyKioskDbContext db)
    {
        
        var errors = new List<Error>();

        
        if (device.DeviceType == DeviceType.Kiosk && await db.Devices.CountAsync(d => d.DeviceType == DeviceType.Recevier) == 0)
        {
                
            errors.Add(Error.Conflict(description:"No receiver added!"));
                
        }

            
        var dbResult = await db.Devices.FirstOrDefaultAsync(d => d.Name.ToLower() == device.Name.ToLower());


        if (dbResult is null)
        {
                
            errors.Add(Error.NotFound());
                
        }
        
        
        return errors;
    }
    
    
    
    private bool ValidateKey(Device device, string textKey)
    {
        
        if (device.IsKeyRevoked || BCrypt.Net.BCrypt.Verify(textKey, device.Key)) //todo: Move to checkPassword with hasher.
        {

            return false;

        }

        
        return true;
    }
}