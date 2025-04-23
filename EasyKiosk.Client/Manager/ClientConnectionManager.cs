using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EasyKiosk.Client.Model;
using EasyKiosk.Core.Model.Requests;
using EasyKiosk.Core.Model.Responses;
using Microsoft.AspNetCore.Components;
using DeviceType = EasyKiosk.Core.Model.Enums.DeviceType;

namespace EasyKiosk.Client.Manager;

public class ConnectionManager
{
    
    public bool IsInitialSetup()
        =>  Preferences.Get(PreferenceNames.ServerAddress, null) == null;
    
    
    
    public async Task RegisterAsync(string ipAdress)
    {
        const string endPoint = "/Device/Register";
        
        var httpClient = new HttpClient();
        var result = await httpClient.GetAsync($"http://{ipAdress}{endPoint}");
        

        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Errors need to be handled!");
        }
        

        var response = await JsonSerializer.DeserializeAsync<DeviceRegisterResponse>(await result.Content.ReadAsStreamAsync());

        
        Preferences.Set(PreferenceNames.ServerAddress, ipAdress);
        Preferences.Set(PreferenceNames.DeviceId, response.Id.ToString());
        Preferences.Set(PreferenceNames.DeviceType, (int)response.Type);
        Preferences.Set(PreferenceNames.AccessKey, response.Token);
        Preferences.Set(PreferenceNames.RefreshKey, response.Refresh);
        
    }



    public async Task<DeviceType> LoginAsync()
    {
        const string endPoint = "/Device/Login";


        var serverAddress = Preferences.Get(PreferenceNames.ServerAddress, null);
        var deviceId = Preferences.Get(PreferenceNames.DeviceId, null);
        var refreshKey = Preferences.Get(PreferenceNames.RefreshKey, null);
        
        
        var loginRequest = new DeviceLoginRequest(Guid.Parse(deviceId), refreshKey);
        var content = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
        

        var httpClient = new HttpClient();
        var result = await httpClient.PostAsync("http://"+ serverAddress + endPoint, content);
        

        if (!result.IsSuccessStatusCode)
        {
            throw new Exception("Handle Login errors!");
        }

        
        var response = await JsonSerializer.DeserializeAsync<DeviceLoginResponse>(await result.Content.ReadAsStreamAsync());


        Preferences.Set(PreferenceNames.AccessKey, response.Token);
        Preferences.Set(PreferenceNames.RefreshKey, response.Refresh);

        
        return (DeviceType)Preferences.Get(PreferenceNames.DeviceType, -1);
    }





    public async Task<string> GetInitialDataAsync()
    {
        var serverAddress = Preferences.Get(PreferenceNames.ServerAddress, null);

        var httpClient = new HttpClient();
        var content = new StringContent(Preferences.Get(PreferenceNames.DeviceType, null), Encoding.UTF8, "application/json");
        
        httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("bearer", Preferences.Get(PreferenceNames.AccessKey, null));


        var response = await httpClient.GetAsync($"http://{serverAddress}/Device/Data");


        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Content);
            throw new Exception("Handle GetDataException");
        }

        return await response.Content.ReadAsStringAsync();
    }
}