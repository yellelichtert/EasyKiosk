using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace EasyKiosk.Core.Managers;

public class SettingsManager<T>
{
    public T Model { get; set; }

    
    public SettingsManager(T model)
    {
        Model = model;
    }


    public virtual async Task SaveChangesAsync()
    {
        try
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            var json = await File.ReadAllTextAsync(filePath); 
            
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            config[nameof(Model)] = Model;
            
            json = JsonSerializer.Serialize(config);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong while trying to save {nameof(Model)}....");
            Console.WriteLine(e);
        }
    }
}