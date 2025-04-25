using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorBootstrap;
using EasyKiosk.Client.HubMethods;
using EasyKiosk.Client.Manager;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
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
    
    [Parameter] public Category[]? Categories { get; set; }
    [Parameter] public Category? SelectedCategory { get; set; }

    [Parameter] public Order? Order { get; set; }
    
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
        var categories = await _connectionManager.GetInitialDataAsync<List<Category>>();

        Console.WriteLine("Done fetching data");
        
        var allProducts = new List<Product>();
        foreach (var category in categories)
        {
            allProducts.AddRange(category.Products);
        }

        var generalCategory = new Category()
        {
            Name = "All",
            Products = allProducts
        };

        categories.Insert(0, generalCategory);
        Categories = categories.ToArray();

        Console.WriteLine("Setting selected categor");
        
        SelectedCategory = Categories[0];

        
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
    
    
    private async Task SendOrderAsync()
    {
        _orderButton.ShowLoading("Sending order");

        Order = await KioskHubController.SendOrderAsync(Order! ,_hubConnection);
        
        _orderButton.HideLoading();
    }
       
}