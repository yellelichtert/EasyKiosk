using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EasyKiosk.Client.Model;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Core.Model.Responses.Device;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using DeviceType = EasyKiosk.Core.Model.Enums.DeviceType;

namespace EasyKiosk.Client.Manager;


/// <summary>
/// Handles everything related to connecting to the server for both Kiosk and Receiver.
/// Only methods that are common between Kiosk and Receiver should be put here.
/// </summary>
public sealed class ConnectionManager
{
    
    public bool IsInitialSetup()
        =>  Preferences.Get(PreferenceNames.ServerAddress, null) == null;
    
    
    
    public async Task<ErrorOr<Success>> RegisterAsync(string ipAddress)
    {
        var response = await MakeRequest<DeviceRegisterResponse>(
            "/Device/Register",
            HttpMethod.Get,
            serverAddress: ipAddress
        );


        if (response.IsError)
        {
            return response.FirstError;
        }

        Preferences.Set(PreferenceNames.ServerAddress, ipAddress);
        Preferences.Set(PreferenceNames.DeviceId, response.Value.Id.ToString());
        Preferences.Set(PreferenceNames.DeviceType, (int)response.Value.Type);
        Preferences.Set(PreferenceNames.AccessKey, response.Value.Token);
        Preferences.Set(PreferenceNames.RefreshKey, response.Value.Refresh);

        return Result.Success;
    }

    
    /// <remarks>
    /// Will reset connection setting if Unauthorized.
    /// </remarks>
    public async Task<ErrorOr<DeviceType>> LoginAsync()
    {
        
        var request = new DeviceLoginRequest()
        {
            Id = Guid.Parse(Preferences.Get(PreferenceNames.DeviceId, null)!),
            Key = Preferences.Get(PreferenceNames.RefreshKey, null)!
        };
        
        
        var response = await MakeRequest<DeviceLoginResponse>(
            "/Device/Login",
            HttpMethod.Post,
            request
        );
        
        
        if (response.IsError)
        {
            if (response.FirstError == Error.Unauthorized())
            {
                Preferences.Clear();
            }
            
            return response.FirstError;
        }

        
        Preferences.Set(PreferenceNames.AccessKey, response.Value.Token);
        Preferences.Set(PreferenceNames.RefreshKey, response.Value.Refresh);

        return (DeviceType)Preferences.Get(PreferenceNames.DeviceType, -1);
    }

    
    
    public async Task<ErrorOr<T>> GetInitialDataAsync<T>()
    {
        var endpointExtension = Preferences.Get(PreferenceNames.DeviceType, -1) == (int)DeviceType.Kiosk
            ? "Kiosk"
            : "Receiver";
        
        
        var response = await MakeRequest<T>(
            $"/Device/Data/{endpointExtension}",
            HttpMethod.Get
        );

        if (response.IsError)
        {
            return response.FirstError;
        }

        return response.Value;
    }



    public async Task<HubConnection> GetHubConnection(NavigationManager navigationManager)
    {
        var connection = new HubConnectionBuilder()
            .WithUrl($"http://{Preferences.Get(PreferenceNames.ServerAddress, null)}/Device/Hub", options =>
            {
                options.Headers.Add("Authorization", $"Bearer {Preferences.Get(PreferenceNames.AccessKey, "")}");
            })
            .Build();

        try
        {
            await connection.StartAsync();
        }
        catch (Exception e)
        {
           navigationManager.NavigateTo("/");
        }
        
        
        connection.Closed += (error) =>
        {
            navigationManager.NavigateTo("/");
            return Task.CompletedTask;
        };
        
        return connection;
        
    }


    
    private async Task<ErrorOr<TResponse>> MakeRequest<TResponse>(string endpoint, HttpMethod method, object? request = null, string? serverAddress = null)
    {
        serverAddress = Preferences.Get(PreferenceNames.ServerAddress, serverAddress);

        if (serverAddress is null)
        {
            throw new NullReferenceException();
        }
       

        var message = new HttpRequestMessage()
        {
            RequestUri = new Uri("Http://"+serverAddress+endpoint),
            Method = method,
        };

        message.Content = request is not null
            ? new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            : null;
        
        
        message.Headers.Authorization =
            new AuthenticationHeaderValue("bearer", Preferences.Get(PreferenceNames.AccessKey, null));
        
            
        HttpResponseMessage response;
        try
        {
            var client = new HttpClient();
            response = await client.SendAsync(message);
        }
        catch (Exception e)
        {
            return Error.Failure(description: "Could not connect to server...");
        }

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return Error.Unauthorized();
            }
            
            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return Error.Forbidden();
            }

            return Error.Unexpected(description: response.ReasonPhrase);
        }


        var responseObj = await JsonSerializer.DeserializeAsync<TResponse>(await response.Content.ReadAsStreamAsync());

            
        if (responseObj is null)
        {
            return Error.NotFound();
        }

        return responseObj;
    }
    
    
}