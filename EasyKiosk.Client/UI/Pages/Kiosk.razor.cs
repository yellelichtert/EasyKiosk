using System.Text.Json;
using BlazorBootstrap;
using EasyKiosk.Core.Entities;
using Microsoft.AspNetCore.Components;
using Button = BlazorBootstrap.Button;

namespace EasyKiosk.Client.UI.Pages;

public partial class Kiosk : ComponentBase
{
    [Parameter] public bool IsLoading { get; set; } = true;
    [Parameter] public List<Category>? Categories { get; set; }
    [Parameter] public Category? SelectedCategory { get; set; }

    [Parameter] public Order? Order { get; set; }
    
    private Offcanvas _orderCanvas = default!;
    private Button _orderButton = default!;
    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        await GetCategoriesAsync();

        IsLoading = false;
    }

    private async Task GetCategoriesAsync()
    {
        var client = new HttpClient();
        var response = await client.GetAsync("http://192.168.99.48:5205/data/kiosk");

        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<List<Category>>(jsonResponse);

        var allProducts = new Category(){Name = "All"};
        foreach (var category in categories)
        {
            allProducts.Products.AddRange(category.Products);
        }
        
        categories.Insert(0, allProducts);
        
        SelectedCategory = allProducts;
        Categories = categories;
    }

    private async Task SendOrderAsync()
    {
        _orderButton.ShowLoading("Sending order");

        await Task.Delay(5000);

        _orderButton.HideLoading();
    }
       
}