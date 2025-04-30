using System.Text.Json;
using EasyKiosk.Core.Model.DTO;
using EasyKiosk.Core.Model.Requests.Order;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.HubMethods;

public static class ReceiverHubMethods
{

    public static void MapReceiverMethods(this HubConnection connection)
    {
        connection.On<string>("ReceiveOrder", (json) =>
        { 
            OnOrderReceived?.Invoke(JsonSerializer.Deserialize<OrderDto>(json));
        });
        
        connection.On<string>("OrderUpdated", (json) =>
        {
            OnOrderUpdated?.Invoke(JsonSerializer.Deserialize<UpdateOrderResponse>(json));
        });
    }


    public delegate void OrderReceived(OrderDto order);
    public static event OrderReceived? OnOrderReceived;


    public delegate void OrderUpdated(UpdateOrderResponse data);
    public static event OrderUpdated? OnOrderUpdated;



    public static async Task<bool> UpdateOrderAsync(string orderNumber, HubConnection connection)
    {
        
        var request = new UpdateOrderRequest()
        {
            orderNumber = orderNumber
        };
        
        
        try
        {
            await connection.InvokeAsync("UpdateOrder", JsonSerializer.Serialize(request));
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
    
    
    
}
