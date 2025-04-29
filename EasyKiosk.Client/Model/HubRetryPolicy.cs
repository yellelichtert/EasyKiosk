using Microsoft.AspNetCore.SignalR.Client;

namespace EasyKiosk.Client.Model;

public class HubRetryPolicy : IRetryPolicy
{
    public TimeSpan? NextRetryDelay(RetryContext retryContext)
    {
        return TimeSpan.FromSeconds(3);
    }
}