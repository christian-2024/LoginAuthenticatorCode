namespace LoginAuthenticatorCode.Shared.Jwt;

public class AppSetting
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}