using Microsoft.AspNetCore.Identity;

namespace EasyKiosk.Server.DependencyInjection;

public static class DependencyInjectionExtentions
{
    public static IServiceCollection ConfigureEasyKioskOptions(this IServiceCollection services, IConfiguration config)
    {


        return services;
    }
}