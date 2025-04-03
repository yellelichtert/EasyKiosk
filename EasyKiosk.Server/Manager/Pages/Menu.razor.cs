using System.Diagnostics;
using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Repositories;
using EasyKiosk.Server.Manager.Components;
using Microsoft.AspNetCore.Components;

namespace EasyKiosk.Server.Manager.Pages;

public partial class Menu : ComponentBase
{

    private IProductRepository _products;
    private ICategoryRepository _categories;

    private List<Product>? _checkedProducts;
    private Product? _selectedProduct;
    private Popup _popup;

    private Func<Task> _formSubmitHandler;
    private bool _formLoading;

   
    
    
    public Menu(IProductRepository products, ICategoryRepository categories)
    {
        _products = products;
        _categories = categories;
    }

    
    
    

    private void EditProductHandler(Product product)
    {
        _selectedProduct = product;

        _formSubmitHandler = UpdateProductInDb;
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

    
    
    
    
    private async Task UpdateProductInDb()
    {
        _formLoading = true;
        await _products.UpdateAsync(_selectedProduct); 
        ResetPopup();
    }

    
    
    
    
    private async Task AddProductToDbAsync()
    {
        _formLoading = true;
        await _products.AddAsync(_selectedProduct);
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
        if (_checkedProducts is null || _checkedProducts.Count == 0)
            return;
        
        {
            await _products.DeleteAsync(_checkedProducts);
        }

        _checkedProducts = new List<Product>();
    }
    
    
    
    
    
    private void ProductCheckboxHandler(Product product)
    {
        if (_checkedProducts == null)
        {
            _checkedProducts = new List<Product>() { product };
        }
        else if (_checkedProducts != null && _checkedProducts.Contains(product))
        {
            _checkedProducts.Remove(product);
        }
        else
        {
            _checkedProducts.Add(product);
        }
       
    }
    
}