using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EasyKiosk.Core.Model;
using EasyKiosk.Core.Model.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EasyKiosk.Core.Factory;

public class DeviceTokenFactory : ITokenFactory<Device>
{

    private readonly TokenOptions _options;

    public DeviceTokenFactory(IOptions<TokenOptions> options)
    {
        _options = options.Value;   
    }
    
    
    public string CreateToken(Device device)
    {
        var secretKeyBytes = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
        var credentials = new SigningCredentials(secretKeyBytes, SecurityAlgorithms.HmacSha256);


        var claims = new[]
        {
            new Claim(ClaimTypes.Name, device.Id.ToString()),
            new Claim(ClaimTypes.Role, device.DeviceType.ToString()),
            new Claim(ClaimTypes.GivenName, device.Name)
        };


        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials:credentials,
            claims: claims
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}