using System.ComponentModel.DataAnnotations;
using EasyKiosk.Client.Manager;
using ErrorOr;
using Microsoft.AspNetCore.Components;
using DeviceType = EasyKiosk.Core.Model.Enums.DeviceType;

namespace EasyKiosk.Client.UI.Pages;

public partial class StartUp : ComponentBase
{
    
    private NavigationManager _navigationManager;
    private ConnectionManager _connectionManager;

    public StartUp(NavigationManager navigationManager, ConnectionManager connectionManager)
    {
        _navigationManager = navigationManager;
        _connectionManager = connectionManager;
    }
    
    private Input _input = new Input();

    private bool _isInitialSetup;
    private bool _isRegistering;

    private bool _hasErrors;
    private string? _errorMessage;
    private string? _countDownMessage;
    private int? _errorCountDown;


    
    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        _isInitialSetup = _connectionManager.IsInitialSetup();
        if (!_isInitialSetup)
        {
            await HandleLogin();
        }
    }


    private async Task HandleRegistration()
    {
        _isRegistering = true;

        var registrationResult = await _connectionManager.RegisterAsync(
            $"{_input.Ip1}.{_input.Ip2}.{_input.Ip3}.{_input.Ip4}:{_input.Port}");


        if (registrationResult.IsError)
        {
            HandleErrors(registrationResult.FirstError);
            return;
        }
        
        
        _isRegistering = false;
        _isInitialSetup = false;

        await HandleLogin();
    }


    private async Task HandleLogin()
    {
        Console.WriteLine("Handeling login");
        var result = await _connectionManager.LoginAsync();


        if (result.IsError)
        {
            HandleErrors(result.FirstError);
            return;
        }
        
        
        if (result.Value == DeviceType.Kiosk)
        {
            _navigationManager.NavigateTo("/Kiosk");
        }
        else
        {
            _navigationManager.NavigateTo("/Receiver");
        }
    }



    private void HandleErrors(Error error)
    {

        _hasErrors = true;
        _errorMessage = error.Description;

        if (error == Error.Unauthorized())
        {
            _errorMessage = "This device is unauthorized...";
            _countDownMessage = "Resetting in: ";
        }
        else if (error == Error.Forbidden())
        {
            _errorMessage = "Connection denied...";
            _countDownMessage = "Returning in";
        }
        else
        {
            _errorMessage = error.Description;
            _countDownMessage = "Trying again in: ";
        }
        
        
        StartErrorCountDown();
    }


    private async Task StartErrorCountDown()
    {
        const int countDownTime = 5;

        for (int i = countDownTime; i > 0; i--)
        {
            _errorCountDown = i;
            await Task.Delay(TimeSpan.FromSeconds(1));
            await InvokeAsync(StateHasChanged);
        }
        
        _navigationManager.NavigateTo("/", true);
    }
    

    private sealed class Input
    {
        [Range(0, 255)] public int Ip1 { get; set; }
        [Range(0, 255)] public int Ip2 { get; set; }
        [Range(0, 255)] public int Ip3 { get; set; }
        [Range(0, 255)] public int Ip4 { get; set; }
        [Range(0, 65535)] public int Port { get; set; }
        
    }
    
}