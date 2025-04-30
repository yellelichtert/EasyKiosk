using System.Security.Claims;
using System.Text.Json;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Requests.Order;
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
        var order = JsonSerializer.Deserialize<OrderRequest>(orderJson);
        
        var result = await _receiverService.PlaceOrderAsync(order);

        
        if (result.IsError)
        {
            await Clients.Caller.SendAsync("Error", result.FirstError.ToString());
            return;
        }
        
        
        await Clients.All.SendAsync("ReceiveOrder", JsonSerializer.Serialize(result.Value));
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOrderNumber", JsonSerializer.Serialize(result.Value.MapToResponse()));
    }


    
    public async Task UpdateOrder(string requestJson)
    {
        var request = JsonSerializer.Deserialize<UpdateOrderRequest>(requestJson);

        var result = await _receiverService.UpdateOrderStateAsync(request);

        
        if (result.IsError)
        {
            return;
        }
        
        
        await Clients.All.SendAsync("OrderUpdated", JsonSerializer.Serialize(result.Value));
    }
    
    
    private string GetDeviceGroupFromClaims()
    {
        return Context.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
    }
    

}