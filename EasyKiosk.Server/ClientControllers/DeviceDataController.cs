using System.Text.Json;
using EasyKiosk.Core.Model.Enums;
using EasyKiosk.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyKiosk.Server.ClientControllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceDataController
{
    private IMenuService _menuService;
    private IOrderService _orderService;

    public DeviceDataController(IMenuService menuService, IOrderService orderService)
    {
        _menuService = menuService;
        _orderService = orderService;
    }
    

    [HttpGet]
    [Route("/Device/Data/{deviceType}")]
    public ActionResult<string> GetDataAsync([FromRoute]int deviceType)
    {

        if ((DeviceType)deviceType == DeviceType.Kiosk)
        {
            var result = _menuService.GetCategories();
            return JsonSerializer.Serialize(result);
        }
        else
        {
            var result = _orderService.GetOpenOrders();
            return JsonSerializer.Serialize(result);
        }
        
    }



}