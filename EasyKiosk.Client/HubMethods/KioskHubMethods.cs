using System;
using System.Text.Json;
using System.Threading.Tasks;
using EasyKiosk.Client.Model;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.HubMethods;

public static class KioskHubController
{

    public static void MapKioskMethods(this HubConnection connection)
    {
        
    }

    public static async Task<OrderResponse> SendOrderAsync(Dictionary<Guid, int> order, HubConnection connection)
    {
        var request = new OrderRequest()
        {
            Order = order,
            DeviceId = Guid.Parse(Preferences.Get(PreferenceNames.DeviceId, null))
        };
        
        Console.WriteLine("Sending order");
        OrderResponse? response = null;
        
        Console.WriteLine("Setting Result method");
        connection.On<string>("ReceiveOrderNumber", (order) =>
        {
            Console.WriteLine("Result method called");

            response = JsonSerializer.Deserialize<OrderResponse>(order);
            connection.Remove("ReceiveOrderNumber");
            Console.WriteLine("Setting Result removed");

        });
        
        
        try
        {
            Console.WriteLine("incoking hub message");

            await connection.InvokeAsync("ReceiveOrder", JsonSerializer.Serialize(request, new JsonSerializerOptions(){IncludeFields = true}));
        }
        catch (Exception e)
        {
            Console.WriteLine("Error : + " + e.Message);
            throw new Exception(e.Message);
        }


        while (response is null)
        {
            Console.WriteLine("Waiting for order");
            await Task.Delay(25);
        }
        
        
        return response;
    }
}