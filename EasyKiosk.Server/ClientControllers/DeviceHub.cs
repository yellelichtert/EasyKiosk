using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace EasyKiosk.Server.ClientControllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceHub : Hub
{
    private const string KioskGroup = "Kiosk";
    private const string ReceiverGroup = "Receiver";
    
    private IOrderRepository _orderRepository;

    public DeviceHub(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
        await Groups.AddToGroupAsync(Context.ConnectionId, GetDeviceGroupFromClaims());
    }


    
    public async Task ReceiveOrder(string orderJson)
    {
        var order = JsonSerializer.Deserialize<Order>(orderJson);
        order.DeviceId = GetGuidFromClaims();

        await _orderRepository.AddAsync(order);
        
        Console.WriteLine("OrderId: " + order.Id);

        orderJson = JsonSerializer.Serialize(order);

        await Clients.All.SendAsync("ReceiveOrder", orderJson);
        await Clients.Client(Context.ConnectionId).SendAsync("ReceiveOrderNumber", orderJson);
    }


    private Guid GetGuidFromClaims()
        => Guid.Parse(Context.User.Claims.First(x => x.Type == ClaimTypes.Name).Value);

    private string GetDeviceGroupFromClaims()
    {
        return Context.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
    }
    

}