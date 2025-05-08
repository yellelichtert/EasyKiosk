using EasyKiosk.Core.Model;
using EasyKiosk.Core.Services;
using EasyKiosk.Server.Manager.Components;
using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;
using EasyKiosk.Server.Service;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace EasyKiosk.Server.Manager.Pages;

public partial class Menu : ComponentBase
{

    private readonly IMenuService _menuService;

    private IReadOnlyList<Category> _categories;
    private IReadOnlyList<Product> _products;
    
    private Product? _selectedProduct;
    private Category? _selectedCategory;
    private List<Product> _checkedProducts;
    
    private Popup _popup;
    private INotificationManager _notificationManager;
    
    private bool _formLoading;
    private bool _imgLoading;
    private string? _formImgPath;

    
    public Menu(IMenuService menuService, INotificationManager notificationManager)
    {
        _menuService = menuService;
        _notificationManager = notificationManager;

        
        _checkedProducts = new();
    }


    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        
        _categories = await _menuService.GetCategoriesAsync();
        _products = await _menuService.GetProductsAsync();
    }


    private void OpenProductForm() => OpenProductForm(new Product());
    private void OpenProductForm(Product product)
    {
        _selectedProduct = product;
        
        _popup.UpdateTitle( product.Id == Guid.Empty ? "Add Product" : "Edit Product");
        _popup.Show();
    }
    
    
    private void OpenCategoryForm() => OpenCategoryForm(new());
    private void OpenCategoryForm(Category category)
    {
        _selectedCategory = category;
        
        _popup.UpdateTitle( category.Id == Guid.Empty ? "Add Category" : "Edit Category");
        _popup.Show();
    }
    
    
    
    private async Task HandleProductSubmit()
    {
        _formLoading = true;
        
        if (_formImgPath is not null)
        {
            _selectedProduct!.Img = _formImgPath;
        }

        ErrorOr<Product> result;
        if (_selectedProduct!.Id == Guid.Empty)
        {
            result = await _menuService.AddProductAsync(_selectedProduct);
        }
        else
        {
            result = await _menuService.UpdateProductAsync(_selectedProduct);
        }


        if (result.IsError)
        {
            AddErrorNotifications(result.Errors);
        }
        else
        {
            _popup.Close();
        }
    }
    
    
    private async Task HandleCategorySubmit()
    {
        _formLoading = true;
        
        if (_formImgPath is not null)
        {
            _selectedCategory!.Img = _formImgPath;
        }

        ErrorOr<Category> result;
        if (_selectedCategory!.Id == Guid.Empty)
        {
            result = await _menuService.AddCategoryAsync(_selectedCategory);
        }
        else
        {
            result = await _menuService.UpdateCategoryAsync(_selectedCategory);
        }


        if (result.IsError)
        {
            AddErrorNotifications(result.Errors);
        }
        else
        {
            _categories = await _menuService.GetCategoriesAsync();
            _popup.Close();
        }
    }


    private void HandleSelectCategory(Category category)
    {
        _selectedCategory = _selectedCategory?.Id == category.Id ? null : category;
        InvokeAsync(StateHasChanged);
    }
    
    private void DeleteCheckedProducts()
    {
        if (_checkedProducts.Count == 0)
            return;

        Task.Run(async () =>
        {
            foreach (var product in _checkedProducts)
            {
                var result = await _menuService.DeleteProductAsync(product);

                if (result.IsError)
                    AddErrorNotifications(result.Errors);
            }

            await InvokeAsync(StateHasChanged);
            _checkedProducts = new List<Product>();
        });
    }


    private void DeleteCategory()
    {
        if (_selectedCategory == null)
            return;

        Task.Run(async () =>
        {
            var result = await _menuService.DeleteCategoryAsync(_selectedCategory);

            if (result.IsError)
            {
                AddErrorNotifications(result.Errors);
            }
            else
            {
                _categories = await _menuService.GetCategoriesAsync();
            }

            
            
            await InvokeAsync(StateHasChanged);
        });
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

    
    private void OnPopupClose()
    {
        _selectedProduct = null;
        _selectedCategory = null;
        _formImgPath = null;
        _formLoading = false;
        
        _notificationManager.Clear();
    }


    private void AddErrorNotifications(List<Error> errors)
    {
        foreach (var error in errors)
        {
            _notificationManager.Add(
                new Notification(error.Description, INotificationManager.Type.Warning
                ));
        }
    }
    
    
    private async Task LoadImage(InputFileChangeEventArgs args)
    {
        _imgLoading = true;
        
        var file = args.File;
        
        using (var memory = new MemoryStream())
        {
            var read = file.OpenReadStream(1024 * 20000);
            
            await read.CopyToAsync(memory);
            
            var base64 = Convert.ToBase64String(memory.ToArray());
            _formImgPath = $"data:image/{file.Name.Split(".")[1]};base64,{base64}";
        }

        _imgLoading = false;
    }
}