namespace EasyKiosk.Server.Service;

public interface IDeviceTokenService
{
    string CreateToken(Guid id, string key);
    Task<(Guid id, string key)> ReadTokenAsync(string token);
}