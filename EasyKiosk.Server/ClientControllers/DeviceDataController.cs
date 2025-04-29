using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyKiosk.Server.ClientControllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DeviceDataController
{
    private IReceiverService _receiverService;
    private IKioskService _kioskService;
    
    public DeviceDataController(IReceiverService receiverService, IKioskService kioskService)
    {
        _receiverService = receiverService;
        _kioskService = kioskService;
    }


    [HttpGet]
    [Route("Device/Data/Receiver")]
    public async Task<ActionResult<ReceiverDataResponse>> GetReceiverDataAsync()
    {
        var openOrders = await _receiverService.GetOpenOrdersAsync();

        return new ReceiverDataResponse()
        {
            OpenOrders = openOrders
        };
    }



    [HttpGet]
    [Route("Device/Data/Kiosk")]
    public async Task<ActionResult<KioskDataResponse>> GetKioskDataAsync()
    {
        var menuData = await _kioskService.GetMenuDataAsync();

        return new KioskDataResponse()
        {
            Categories = menuData.categories,
            Products = menuData.products
        };
    }
}