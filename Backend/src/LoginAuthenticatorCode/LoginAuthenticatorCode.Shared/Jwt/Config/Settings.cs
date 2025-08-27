namespace LoginAuthenticatorCode.Shared.Jwt.Config;

public static class Settings
{
    public static string SecretKey = Guid.NewGuid().ToString();
}