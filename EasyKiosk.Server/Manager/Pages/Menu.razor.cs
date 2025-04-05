using System.Diagnostics;
using System.IO.Pipelines;
using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Server.Manager.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EasyKiosk.Server.Manager.Pages;

public partial class Menu : ComponentBase
{

    private IProductRepository _products;
    private ICategoryRepository _categories;

    private List<Product> _checkedProducts;
    private Product? _selectedProduct;
    private Popup _popup;

    private Func<Task> _formSubmitHandler;
    private bool _formLoading;

    private string? _formImgPath;
    private long _maxfileSize = 1024 * 400;
    private bool _imageLoading;
    
    
    public Menu(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;

        _checkedProducts = new();
    }

    
    
    

    private void EditProductHandler(Product product)
    {
        _selectedProduct = product;

        _formSubmitHandler = UpdateProductInDbAsync;
        _popup.UpdateTitle("Edit Product");
        _popup.Show();
    }

    
    
    
    private void AddProductHandler()
    {
        _selectedProduct = new Product();

        _formSubmitHandler = AddProductToDbAsync;
        _popup.UpdateTitle("New Product");
        _popup.Show();
    }

    
    
    
    
    private async Task UpdateProductInDbAsync()
    {
        _formLoading = true;
        await _products.UpdateAsync(_selectedProduct!); 
        ResetPopup();
        
        //todo: decode image string back to original bytes
        //todo: upload file to server
    }

    
    
    
    
    private async Task AddProductToDbAsync()
    {
        _formLoading = true;
        await _products.AddAsync(_selectedProduct!);
        ResetPopup();
    }


    
    
    
    private void ResetPopup()
    {
        _formLoading = false;
        _popup.Close();
        _selectedProduct = null;
    }

    
    
    
    private async Task DeleteCheckedProducts()
    {
        if (_checkedProducts.Count == 0)
            return;
        
        if (_checkedProducts.Count == 1)
        {
            await _products.DeleteAsync(_checkedProducts.First());
        }
        else
        { 
             await _products.DeleteAsync(_checkedProducts);
        }

        _checkedProducts = new List<Product>();
    }
    
    
    
    
    
    private void ProductCheckboxHandler(Product product)
    {
        if (_checkedProducts.Contains(product))
        {
            _checkedProducts.Remove(product);
        }
        else
        { 
            _checkedProducts.Add(product);
        }
    }


    private async Task LoadImage(InputFileChangeEventArgs args)
    {
        var file = args.File;
        
        //Iets anders als memoryStream?
        //memorystream maak foto heeeeel groot
        
        using (var memory = new MemoryStream())
        {
            var read = file.OpenReadStream(_maxfileSize);
            
            await read.CopyToAsync(memory);
            
            var base64 = Convert.ToBase64String(memory.ToArray());
            _formImgPath = $"data:image/{file.Name.Split(".")[1]};base64,{base64}";
        }
    }
}