using System;
using System.Text.Json;
using System.Threading.Tasks;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Entities;
using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.HubMethods;

public static class KioskHubController
{

    public static void MapKioskMethods(this HubConnection connection)
    {
        
    }

    public static async Task<Order> SendOrderAsync(Order order, HubConnection connection)
    {
        Order? completedOrder = null;
        
        connection.On<string>("ReceiveOrderNumber", (order) =>
        {
            completedOrder = JsonSerializer.Deserialize<Order>(order);
            connection.Remove("ReceiveOrderNumber");
        });
        
        
        try
        {
            await connection.InvokeAsync("ReceiveOrder", JsonSerializer.Serialize(order));
        }
        catch (Exception e)
        {
            connection.Remove("");
            
            throw new Exception(e.Message);
        }


        while (completedOrder is null)
        {
            Console.WriteLine("Waiting for order");
            await Task.Delay(25);
        }
        
        return completedOrder;
    }
}