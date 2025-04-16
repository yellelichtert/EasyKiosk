using EasyKiosk.Server.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyKiosk.Server.Manager.Components.Common.Notifications.Base;

public class Notification : DynamicComponent
{
    public override Type ComponentType { get; } = typeof(NotificationComponent);
    
    public INotificationManager.Type Type { get; }
    public string Message;


    public Notification(string message, INotificationManager.Type type = INotificationManager.Type.Info)
    {
        Message = message;
        Type = type;
    }
}