using EasyKiosk.Server.Manager.Components;
using EasyKiosk.Server.Manager.Components.Common.Notifications.Base;

namespace EasyKiosk.Server.Service;

public class NotificationManager : INotificationManager
{
    private List<Notification> _notifications = new();
    

    public event Action? OnChange;

    
    
    public IReadOnlyCollection<Notification> GetNotifications()
        => _notifications;
    
    
    public void Add(Notification notification)
    {
        Console.WriteLine("Adding Notification...");
        
        _notifications.Add(notification);
        
        OnChange?.Invoke();
        
        Console.WriteLine("Done Adding Notification...");

    }

    public void Remove(Notification notification)
    {
        Console.WriteLine("Removing Notification...");

        _notifications.Remove(notification);
        OnChange?.Invoke();
        
        Console.WriteLine("Done Removing Notification...");
    }

    public void Clear()
    {
        _notifications = new List<Notification>();
    }
    
}