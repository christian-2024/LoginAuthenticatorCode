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
    public static string GenerateJwtToken(List<Claim> additionalClaims, JwtSettings jwtSettings)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(additionalClaims),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.TokenExpirationInMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}