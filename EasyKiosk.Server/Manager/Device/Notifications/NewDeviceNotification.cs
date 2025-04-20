using EasyKiosk.Core.Model.Responses;
using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;



namespace EasyKiosk.Server.Manager.Device.Notifications;

public sealed class NewDeviceNotification : Notification
{
    public override Type ComponentType { get; } = typeof(NewDeviceNotificationComponent);
    public TaskCompletionSource<DeviceRegisterResponse?> Tcs { get; }
    
    
    public NewDeviceNotification(TaskCompletionSource<DeviceRegisterResponse?> tcs, string message =  "A new device is trying to connect, would you like to connect?") 
        : base(message)
    {
        Tcs = tcs;
    }
    
    
}