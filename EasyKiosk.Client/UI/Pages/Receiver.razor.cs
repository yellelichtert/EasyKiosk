using BlazorBootstrap;
using EasyKiosk.Client.HubMethods;
using EasyKiosk.Client.Manager;
using EasyKiosk.Client.UI.Components;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Button = BlazorBootstrap.Button;


namespace EasyKiosk.Client.UI.Pages;

public partial class Receiver : ComponentBase
{
    [CascadingParameter] public LoadingScreen LoadingScreen { get; set; }
    
    private ConnectionManager _connectionManager;
    private NavigationManager _navigationManager;
    
    private HubConnection _connection;
    
    private Modal _modal;
    private Button _updateButton; 
        
    private string? _selectedOrder = null;
    
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
        Orders = data.Value.OpenOrders.ToList();
        
        
        _connection = await _connectionManager.GetHubConnection(_navigationManager);
        _connection.MapReceiverMethods();
        
        
        await InvokeAsync(StateHasChanged);


        ReceiverHubMethods.OnOrderReceived += HandleNewOrder;
        ReceiverHubMethods.OnOrderUpdated += HandleOrderUpdated;

    }


    private void HandleNewOrder(OrderDto order)
    {
        Orders.Add(order);
        InvokeAsync(StateHasChanged);
    }

    
    
    private void HandleOrderUpdated(UpdateOrderResponse data)
    {
        var order = Orders?.FirstOrDefault(o => o.OrderNumber == data.orderNumber);

        if (data.State == OrderState.Finished && order is not null)
        {
            Orders.Remove(order);
        }
        else if (order is not null )
        {
            order.State = data.State;
        }
        
        
        InvokeAsync(StateHasChanged);
    }


    private async Task HandleUpdateOrderRequest()
    {
        _updateButton.Loading = true;

        await ReceiverHubMethods.UpdateOrderAsync(_selectedOrder, _connection);
        await _modal.HideAsync();
        
        _updateButton.Loading = false;
    }
    
    
    private async Task HandleOrderTap(string orderNumber)
    {
        _selectedOrder = orderNumber;
        await _modal.ShowAsync();
    }

    
    private async Task OnModelShown()
    {
        await Task.Delay(TimeSpan.FromSeconds(3));

        if (!_updateButton.Loading)
        {
            await _modal.HideAsync();
        }
        
    }
}