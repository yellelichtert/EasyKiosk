using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Requests;
using EasyKiosk.Core.Responses;
using EasyKiosk.Core.Services;
using EasyKiosk.Server.Manager.Device;
using EasyKiosk.Server.Manager.Device.Notifications;
using EasyKiosk.Server.Options;
using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyKiosk.Server.ClientControllers;

[ApiController]
public class ClientAuthController : Controller
{
    private DeviceAuthOptions _options;
    
    private INotificationManager _notificationManager;
    private IDeviceService _deviceService;
    
    public ClientAuthController
        (
            IOptions<DeviceAuthOptions> options,
            IDeviceService deviceService,
            INotificationManager notificationManager
        )
    {
        _options = options.Value;
        _deviceService = deviceService;
        _notificationManager = notificationManager;
    }


    [HttpGet]
    [Route("/Device/Login")]
    public async Task<ActionResult<DeviceLoginResponse>> LoginAsync([FromBody]DeviceLoginRequest request)
    {
        var result = await _deviceService.ValidateLoginAsync(request);

        if (result.IsError)
        {
            return Unauthorized();
        }
        
        return new DeviceLoginResponse(Generatetoken(result.Value), result.Value.DeviceType);
    }
    
    
    
    [HttpGet]
    [Route("/Device/Register")]
    public async Task<ActionResult<DeviceRegisterResponse>> RegisterAsync()
    {
        //todo: Move to tcs to error or 
        TaskCompletionSource<(Device, string)?> tcs = new();
        NewDeviceNotification notification = new(tcs);
        
        _notificationManager.Add(notification);
        

        var result = await tcs.Task;
        
        if (result is null)
        {
            return Forbid();
        }
        
        return new DeviceRegisterResponse(result.Value.Item1.Id, result.Value.Item2);
    }
    
    
    
    private string Generatetoken(Device device)
    {

        var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha256);


        var claims = new[]
        {
            new Claim(ClaimTypes.Name, device.Id.ToString()),
            new Claim(ClaimTypes.Role, device.DeviceType.ToString()),
            new Claim(ClaimTypes.GivenName, device.Name)
        };


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials:credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    
    
    
    
    // [HttpGet]
    // [Route("/data/kiosk")]
    // public List<Category> GetKioskData()
    // {  
    //     var result = _menuService.GetCategories().ToList();
    //     
    //     Console.WriteLine("DB RESULT COUNT: "+ result.Count);
    //     
    //     return result;
    // }
    
}