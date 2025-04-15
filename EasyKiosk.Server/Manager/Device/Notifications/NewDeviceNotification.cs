using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;
using EasyKiosk.Server.Service;

namespace EasyKiosk.Server.Manager.Device.Notifications;

public sealed class NewDeviceNotification : Notification
{
    
    private const string DefaultMessage = "A device is trying to connect, allow Connection?";
    
    public override Type ComponentType { get; } = typeof(NewDeviceNotificationComponent);
    public TaskCompletionSource<bool> Tcs { get;}
    
    
    public NewDeviceNotification(TaskCompletionSource<bool> tcs, string message = DefaultMessage)
        : base(true, message)
    {
        Tcs = tcs;
        ComponentParameters.Add("Model", this);
    }
    
    
    public void HandleResponse(bool response)
    {
        Tcs.SetResult(response);
        Dismiss();
    }
}