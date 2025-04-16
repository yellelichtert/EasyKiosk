using EasyKiosk.Server.Options;

namespace EasyKiosk.Server.DependencyInjection;

public static class DependencyInjectionExtentions
{
    public static IServiceCollection ConfigureEasyKioskOptions(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<DeviceAuthOptions>(
            config.GetSection(nameof(DeviceAuthOptions)));

        return services;
    }
}