using System.Security.Claims;
using System.Text.Json;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EasyKiosk.Server.ClientControllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceHub : Hub
{
    private const string KioskGroup = "Kiosk";
    private const string ReceiverGroup = "Receiver";

    private IReceiverService _receiverService;

    public DeviceHub(IReceiverService receiverService)
    {
        _receiverService = receiverService;
    }
    
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Groups.AddToGroupAsync(Context.ConnectionId, GetDeviceGroupFromClaims());
    }


    
    public async Task ReceiveOrder(string orderJson)
    {
        Console.WriteLine(orderJson);
        
        var order = JsonSerializer.Deserialize<OrderRequest>(orderJson);
        
        var result = await _receiverService.PlaceOrderAsync(order);

        if (result.IsError)
        {
            throw new Exception("ERROR UNHANDLED");
            //Send error message?
        }
        
        
        await Clients.All.SendAsync("ReceiveOrder", JsonSerializer.Serialize(result.Value));
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOrderNumber", JsonSerializer.Serialize(result.Value.MapToResponse()));
    }

    
    
    
    private string GetDeviceGroupFromClaims()
    {
        return Context.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
    }
    

}