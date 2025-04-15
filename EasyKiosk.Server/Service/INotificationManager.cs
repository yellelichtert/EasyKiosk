using EasyKiosk.Server.Manager.Components;
using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;

namespace EasyKiosk.Server.Service;

public interface INotificationManager
{
    event Action OnChange;
    
    public IReadOnlyCollection<Notification> GetNotifications();
    public void Add(Notification notification);
    public void Remove(Notification notification);
    
    public void Clear();
    
    public enum Type { Succes, Info, Warning }
}


public interface IInputNotification<TInputResult>
{
    public Task<TInputResult> WaitForInput(); 
}