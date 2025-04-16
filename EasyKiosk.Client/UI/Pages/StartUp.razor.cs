using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.UI.Pages;

public partial class StartUp : ComponentBase
{
    private const string ServerAdress = "http://192.168.99.48:5205";
    private const string TokenEndpoint = "/Device/GetToken";
    private const string HubEndPoint = "/Hub";

    private HubConnection _connection;

    
    [Parameter] public bool IsRequestingToken { get; set; } = true;
    [Parameter] public bool IsConnecting { get; set; }
    [Parameter] public string? ErrorMessage { get; set; }

    protected override async Task OnInitializedAsync()
    { 
        await base.OnInitializedAsync();

        var token = await RequestTokenAsync();
        Console.WriteLine("Token: " + token);
        
        IsRequestingToken = false;
        IsConnecting = true;

        await ConnectToHubAsync(token);

        IsConnecting = false;
    }



    private async Task<string?> RequestTokenAsync()
    {
        var client = new HttpClient();
        var result = await client.GetAsync($"{ServerAdress}{TokenEndpoint}");

        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadAsStringAsync();
        }
        else
        {
            ErrorMessage = "Something went wrong....";
            return null;
        }
        
      
    }


    private async Task ConnectToHubAsync(string token)
    {
        Console.WriteLine("Building connection....");
        var connection = new HubConnectionBuilder()
            .WithUrl($"{ServerAdress}{HubEndPoint}", options =>
            {
                options.Headers.Add("Authorization", token);
            })
            .WithAutomaticReconnect()
            .Build();


        connection.Closed += ConnectionOnClosed;
        
        Task ConnectionOnClosed(Exception? arg)
        {
            Console.WriteLine("Connection closed....");
            return Task.CompletedTask;
        }
        
        Console.WriteLine("Starting Connection");
        await connection.StartAsync();
        
        Console.WriteLine("ConnectionState = " + connection.State);
        
    }
}