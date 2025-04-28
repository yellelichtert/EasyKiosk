using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorBootstrap;
using EasyKiosk.Client.HubMethods;
using EasyKiosk.Client.Manager;
using EasyKiosk.Client.Model;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Button = BlazorBootstrap.Button;

namespace EasyKiosk.Client.UI.Pages;

public partial class Kiosk : ComponentBase
{
    private ConnectionManager _connectionManager;
    
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public string LoadingMessage { get; set; }
    [Parameter] public Dictionary<Guid, int> Order { get; set;}
    [Parameter] public OrderResponse? OrderResponse { get; set; }
    
    
    private CategoryDto[]? _categories;
    private ProductDto[]? _products;
    private Guid? _selectedCategory = Guid.Empty;
    
    
    private Offcanvas _orderCanvas = default!;
    private Button _orderButton = default!;

    private HubConnection _hubConnection;

    public Kiosk(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }
    
    
    
    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        
        
        await base.OnInitializedAsync();
        
        LoadingMessage = "Fetching data...";
        
        var data = await _connectionManager.GetInitialDataAsync<KioskDataResponse>();
        
        _categories = data.Categories;
        _products = data.Products;
        
        
        Console.WriteLine("Done setting category");
        
        LoadingMessage = "Connecting to hub...";
        
        _hubConnection = await _connectionManager.GetHubConnection();
        
        _hubConnection.Reconnecting += (error) =>
        {
            LoadingMessage = "Connecting to hub...";
            IsLoading = true;
            return Task.CompletedTask;
        };
        
        _hubConnection.Reconnected += (error) =>
        {
            IsLoading = false;
            return Task.CompletedTask;
        };
        
        IsLoading = false;
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
        LoadingMessage = "Sending order...";
        IsLoading = true;
        OrderResponse = await KioskHubController.SendOrderAsync(Order ,_hubConnection);
        IsLoading = false;

        StartResetCountDown();
    }

    


    private async Task StartResetCountDown()
    {
        await Task.Delay(TimeSpan.FromSeconds(10));
        Order = null;

        await InvokeAsync(StateHasChanged);
    }
}