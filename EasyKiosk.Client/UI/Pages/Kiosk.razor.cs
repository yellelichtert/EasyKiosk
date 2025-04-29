using BlazorBootstrap;
using EasyKiosk.Client.HubMethods;
using EasyKiosk.Client.Manager;
using EasyKiosk.Client.UI.Components;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;



namespace EasyKiosk.Client.UI.Pages;

public partial class Kiosk : ComponentBase
{
    [Parameter] public Dictionary<Guid, int>? Order { get; set;}
    
    [Parameter] public OrderResponse? OrderResponse { get; set; }
    
    
    [CascadingParameter] public LoadingScreen LoadingScreen { get; set; }
    private Offcanvas _orderCanvas;
    
    private ConnectionManager _connectionManager;
    private NavigationManager _navigationManager;
    
    private HubConnection _hubConnection;
    
    private CategoryDto[]? _categories;
    
    private ProductDto[]? _products;
    
    private Guid _selectedCategory = Guid.Empty;
    
    
    public Kiosk(ConnectionManager connectionManager, NavigationManager navigationManager)
    {
        _connectionManager = connectionManager;
        _navigationManager = navigationManager;
    }


    protected override  async Task OnInitializedAsync()
    {
        LoadingScreen.Show("Fetching data...");

        var data = await _connectionManager.GetInitialDataAsync<KioskDataResponse>();

        if (data.IsError)
        {
            LoadingScreen.Show("error while fetching data... ");
        }

        while (data.IsError)
        {
            await _connectionManager.GetInitialDataAsync<KioskDataResponse>();
        }

        _categories = data.Value.Categories;
        _products = data.Value.Products;

        
        LoadingScreen.Show("Connecting to hub...");
        
        _hubConnection = await _connectionManager.GetHubConnection(_navigationManager);

        
        _hubConnection.Closed += (error) =>
        {
            _navigationManager.NavigateTo("/");
            return Task.CompletedTask;
        };
       

        LoadingScreen.Hide();
    }

    
    private ProductDto[] GetVisibleProducts()
    {
        if (_selectedCategory == Guid.Empty)
        {
            return _products;
        }
       
        return _products.Where(p => p.CategoryId == _selectedCategory).ToArray();
    }


    private ProductDto GetProduct(Guid id)
        => _products.First(p => p.Id == id);


    private decimal GetFullPrice()
    {
        decimal sum = 0;
        foreach (var item in Order)
        {
            sum += GetProduct(item.Key).Price * item.Value;
        }

        return sum;
    }
        
    

    private void AddProduct(Guid id)
    {
        if (Order.ContainsKey(id))
        {
            Order[id]++;
        }
        else
        {
            Order.Add(id, 1);
        }
        
        InvokeAsync(StateHasChanged);
    }

    private void RemoveProduct(Guid id)
    {
        if (Order.ContainsKey(id))
        {
            Order[id]--;

            if (Order[id] == 0)
            {
                Order.Remove(id);
            }
            
            InvokeAsync(StateHasChanged);
        }
    }
    
    
    private async Task SendOrderAsync()
    {
        LoadingScreen.Show("Sending order...");
        
        OrderResponse = await KioskHubController.SendOrderAsync(Order ,_hubConnection);
        
        LoadingScreen.Hide();
        
        StartResetCountDown();
    }

    


    private async Task StartResetCountDown()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        
        Order = null;
        OrderResponse = null;
        
        await InvokeAsync(StateHasChanged);
    }
    
}