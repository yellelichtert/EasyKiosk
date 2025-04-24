using System;

using System.Threading.Tasks;
using BlazorBootstrap;
using EasyKiosk.Client.Manager;
using EasyKiosk.Core.Model.Entities;
using Microsoft.AspNetCore.Components;
using Button = BlazorBootstrap.Button;

namespace EasyKiosk.Client.UI.Pages;

public partial class Kiosk : ComponentBase
{
    private ConnectionManager _connectionManager;
    
    [Parameter] public bool IsLoading { get; set; } = true;
    [Parameter] public Category[]? Categories { get; set; }
    [Parameter] public Category? SelectedCategory { get; set; }

    [Parameter] public Order? Order { get; set; }
    
    private Offcanvas _orderCanvas = default!;
    private Button _orderButton = default!;


    public Kiosk(ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }
    
    
    
    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine("Initilizing Kiosk");
        
        await base.OnInitializedAsync();
        
        Console.WriteLine("Fetching data...");

        Categories = await _connectionManager.GetInitialDataAsync<Category[]>();
        
        Console.WriteLine("Done Fetching data \n Categories count: " + Categories.Length);
        
        IsLoading = false;
    }
    
    
   
    

    private async Task SendOrderAsync()
    {
        _orderButton.ShowLoading("Sending order");

        await Task.Delay(5000);

        _orderButton.HideLoading();
    }
       
}