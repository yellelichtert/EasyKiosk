using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Services;
using EasyKiosk.Server.Manager.Device.Notifications;
using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyKiosk.Server.ClientControllers;

[AllowAnonymous]
[ApiController]
public class ClientAuthController : Controller
{
    
    private INotificationManager _notificationManager;
    private IDeviceService _deviceService;
    
    public ClientAuthController
        (
            IDeviceService deviceService,
            INotificationManager notificationManager
        )
    {
        _deviceService = deviceService;
        _notificationManager = notificationManager;
    }
    

    [HttpGet]
    [Route("/Device/Login")]
    public async Task<ActionResult<DeviceLoginResponse>> LoginAsync([FromBody]DeviceLoginRequest request)
    {
        var result = await _deviceService.LoginAsync(request);

        if (result.IsError)
        {
            return Unauthorized($"{result.Errors.First().Description}");
        }
        
        return result.Value;
    }
    
    
    
    [HttpGet]
    [Route("/Device/Register")]
    public async Task<ActionResult<DeviceRegisterResponse>> RegisterAsync()
    {
        TaskCompletionSource<DeviceRegisterResponse?> tcs = new();
        NewDeviceNotification notification = new(tcs);
        
        _notificationManager.Add(notification);
        

        var result = await tcs.Task;
        
        if (result is null)
        {
            return Forbid();
        }
        
        return result;
    }
}