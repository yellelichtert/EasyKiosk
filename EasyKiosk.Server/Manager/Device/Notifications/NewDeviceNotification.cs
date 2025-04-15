using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;


namespace EasyKiosk.Server.Manager.Device.Notifications;

public sealed class NewDeviceNotification : Notification
{
    public override Type ComponentType { get; } = typeof(NewDeviceNotificationComponent);

    
    
    public NewDeviceNotification()
    {
        Message = "A new device is trying to connect, would you like to connect?";
    }
    
    
    
    private TaskCompletionSource<bool> _tcs;
    public TaskCompletionSource<bool> Tcs
    {
        get => _tcs ?? throw new NullReferenceException("Completion Source (Tcs) cannot be null");
        set => _tcs = value;
    }
    
    
    
    public void HandleResponse(bool response)
    {
        Tcs.SetResult(response);
        Dismiss();
    }
}