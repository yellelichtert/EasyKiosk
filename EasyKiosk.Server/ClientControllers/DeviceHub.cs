using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.AspNetCore.SignalR;

namespace EasyKiosk.Server.ClientControllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceHub : Hub
{
    
    
    public override async Task OnConnectedAsync()
    {

       
        
        Console.WriteLine("Device Connected => " +  Context.GetHttpContext().User.Claims.First());
        
        await base.OnConnectedAsync();
    }
}