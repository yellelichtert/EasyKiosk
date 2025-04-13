using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.SignalR;

namespace EasyKiosk.Server.ClientControllers;

public class DeviceHub : Hub
{
    private ITokenService _tokenService;
    

    public DeviceHub(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }
    
    
    public override async Task OnConnectedAsync()
    {

        Console.WriteLine("Device Connected");
        
        await base.OnConnectedAsync();
    }
}