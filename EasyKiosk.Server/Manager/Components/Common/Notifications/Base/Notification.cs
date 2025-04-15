using EasyKiosk.Server.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyKiosk.Server.Manager.Components.Common.Notifications.Base;

public class Notification : DynamicComponent
{
    public override Type ComponentType { get; } = typeof(NotificationComponent);
    public INotificationManager.Type Type { get; set; } = INotificationManager.Type.Info;
    
    
    
    protected string _message;
    public string Message
    {
        get => _message ?? throw new NullReferenceException("Message cannot be null");
        init => _message = value;
    }
    
    

    public event Action<Notification> OnDismiss;
    public virtual void Dismiss()
        => OnDismiss?.Invoke(this);
    
}