using System.Security.Authentication;
using EasyKiosk.Core.Enums;
using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.SignalR;

namespace EasyKiosk.Server.Filter;

public class HubFilter(ITokenService tokenService) : IHubFilter
{
    
    public async Task OnConnectedAsync(HubLifetimeContext context, Func<HubLifetimeContext, Task> next)
    {
        var token = context.Context.GetHttpContext().Request.Headers.Authorization.First();

        var device = await tokenService.ValidateTokenAsync(token);
        if (device is not null)
        {
            var connectionId = 
            
            context.Hub.Groups.AddToGroupAsync(context.Context.ConnectionId, device.DeviceType == DeviceType.Kiosk
            ? "Kiosk"
            : "Receiver");
            
            await next(context);
        }
        else
        {
            throw new InvalidCredentialException();
        }
    }
}