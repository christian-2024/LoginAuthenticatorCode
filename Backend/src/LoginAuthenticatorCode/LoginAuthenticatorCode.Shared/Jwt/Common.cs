using LoginAuthenticatorCode.Shared.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Atomo.Hub.Crm.Shared.Jwt;

public static class Common
{
    /// <summary>
    /// Generate Jwt Token
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="appSettingSecret"></param>
    /// <returns></returns>
    public static string GenerateJwtToken(List<Claim> additionalClaims, AppSetting appSetting)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(appSetting.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(additionalClaims),
            Issuer = appSetting.Issuer,
            Audience = appSetting.Audience,
            Expires = DateTime.UtcNow.AddMinutes(appSetting.TokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}