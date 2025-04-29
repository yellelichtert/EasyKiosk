using System.Text.Json;
using EasyKiosk.Client.Manager;
using EasyKiosk.Client.UI.Components;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.UI.Pages;

public partial class Receiver : ComponentBase
{
    [CascadingParameter] public LoadingScreen LoadingScreen { get; set; }
    
    private ConnectionManager _connectionManager;
    private NavigationManager _navigationManager;
    
    private HubConnection _connection;

    
    
    [Parameter]public List<OrderDto>? Orders { get; set; }
    
    
    public Receiver(ConnectionManager connectionManager, NavigationManager navigationManager)
    {
        _connectionManager = connectionManager;
        _navigationManager = navigationManager;
    }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        var data = await _connectionManager.GetInitialDataAsync<ReceiverDataResponse>();
        
        _connection = await _connectionManager.GetHubConnection(_navigationManager);
        Orders = data.Value.OpenOrders.ToList();
        

        await InvokeAsync(StateHasChanged);
        
        
        Console.WriteLine("Hub connection state => " + _connection.State);

        _connection.On<string>("ReceiveOrder", (orderJson) =>
        {
            Console.WriteLine("Order Received");
            Console.WriteLine("OrderJson => " + orderJson);
            
            var order = JsonSerializer.Deserialize<OrderDto>(orderJson);
            Orders.Add(order);
            
            InvokeAsync(StateHasChanged);
        });
    }
}