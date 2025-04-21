using System;
using System.IO;
using System.Reflection;
using EasyKiosk.Core.Managers;
using EasyKiosk.Core.Model.Options;
using EasyKiosk.Core.Model.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Device = EasyKiosk.Core.Model.Entities.Device;


namespace EasyKiosk.Client;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});
		
	
			
		
		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddBlazorBootstrap();


		
		

		var environment = Environment.GetEnvironmentVariable("environment");
		
		
		builder.Services.AddSingleton<SettingsManager<DeviceSettings>>();

		builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.AddJsonFile($"appsettings.{environment}.json", optional: true)
			.Build();
		
		
		var deviceSetting = new DeviceSettings();
		builder.Configuration.Bind("DeviceSettings",deviceSetting);
		builder.Services.AddSingleton(deviceSetting);
		
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
