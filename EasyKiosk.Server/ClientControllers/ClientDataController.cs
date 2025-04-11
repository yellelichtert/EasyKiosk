using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyKiosk.Server.ClientControllers;

[ApiController]
public class ClientDataController : Controller
{
    private IMenuService _menuService;
    
    public ClientDataController(IMenuService menuService)
    {
        _menuService = menuService;
    }
    
    [HttpGet]
    [Route("/data/kiosk")]
    public List<Category> GetKioskData()
    {  
        var result = _menuService.GetCategories().ToList();
        
        Console.WriteLine("DB RESULT COUNT: "+ result.Count);
        
        return result;
    }
    
}