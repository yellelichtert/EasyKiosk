using System;
using System.Collections.Generic;
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
    
    [Parameter] public bool IsLoading { get; set; }
    [Parameter] public string LoadingMessage { get; set; }    
    
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
        IsLoading = true;
        
        await base.OnInitializedAsync();
        
        LoadingMessage = "Fetching data...";
        var categories = await _connectionManager.GetInitialDataAsync<List<Category>>();

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

        SelectedCategory = categories[0];
        
        IsLoading = false;
    }
    
    
   
    
    private async Task SendOrderAsync()
    {
        _orderButton.ShowLoading("Sending order");

        await Task.Delay(5000);

        _orderButton.HideLoading();
    }
       
}