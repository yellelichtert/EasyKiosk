using EasyKiosk.Server.Service;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EasyKiosk.Server.Manager.Components.Common.Notifications.Base;

public class Notification : DynamicComponent
{
    public override Type ComponentType { get; } = typeof(NotificationComponent);

    public INotificationManager.Type Type { get;}
    public string Message { get; }

    public event Action<Notification> OnDismiss;

    protected Notification(bool populateParameters, string message, INotificationManager.Type type = INotificationManager.Type.Info)
    {
        Type = type;
        Message = message;
    }
    
    public Notification(string message, INotificationManager.Type type = INotificationManager.Type.Info)
    {
        Type = type;
        Message = message;
        
        ComponentParameters.Add("Model", this);
    }



    public virtual void Dismiss()
    {
        OnDismiss?.Invoke(this);
        Console.WriteLine("Base called EVENT");
    }
        
}