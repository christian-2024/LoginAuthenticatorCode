using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAuthenticatorCode.Shared.Jwt;

    public static class Common
    {
        /// <summary>
        /// Generate Jwt Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="appSettingSecret"></param>
        /// <returns></returns>
        public static string GenerateJwtToken(long userId, AppSetting appSettings)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                //new Claim(ClaimTypes.Role, ((int)perfil).ToString())
            }),
                Issuer = appSettings.Issuer,
                Audience = appSettings.Audience,
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
