using EasyKiosk.Core.Entities;
using EasyKiosk.Core.Services;
using EasyKiosk.Server.Service;
using Microsoft.AspNetCore.Mvc;

namespace EasyKiosk.Server.ClientControllers;

[ApiController]
public class ClientDataController : Controller
{
    private IMenuService _menuService;
    private IDeviceService _deviceService;
    private ITokenService _tokenService;
    
    public ClientDataController(IMenuService menuService, IDeviceService deviceService, ITokenService tokenService)
    {
        _menuService = menuService;
        _deviceService = deviceService;
        _tokenService = tokenService;
    }


    [HttpGet]
    [Route("/Device/GetToken")]
    public async Task<string> GetToken()
    {
        Console.WriteLine("Get token endpoint hit");
        
        var devices = _deviceService.GetAllDevices();
        var first = devices.First();
        
        //Generate api key
        return _tokenService.GenerateToken(first, first.Key);
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