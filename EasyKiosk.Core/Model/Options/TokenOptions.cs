namespace EasyKiosk.Core.Model.Options;

public class TokenOptions
{
    public static string tokenOptions = "TokenOptions";
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
}