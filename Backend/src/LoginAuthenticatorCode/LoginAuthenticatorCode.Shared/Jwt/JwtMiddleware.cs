using LoginAuthenticatorCode.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LoginAuthenticatorCode.Shared.Jwt;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSetting _appSetting;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSetting> appSetting)
    {
        _next = next;
        _appSetting = appSetting.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
            await AttachUserToContextAsync(context, userService, token);

        await _next(context);
    }

    private async Task AttachUserToContextAsync(HttpContext context, IUserService _serviceUser, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSetting.SecretKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            // attach user to context on successful Jwt validation
            if (userId > 0)
                context.Items["User"] = await _serviceUser.GetByIdAsync(userId); /* unitOfWork.Users.GetById(userId).Result;*/
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}