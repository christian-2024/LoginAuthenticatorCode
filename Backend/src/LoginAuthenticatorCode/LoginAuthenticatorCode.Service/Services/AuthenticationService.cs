using Atomo.Hub.Crm.Shared.Jwt;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using LoginAuthenticatorCode.Domain.Interfaces.Service;
using LoginAuthenticatorCode.Shared.Claims;
using LoginAuthenticatorCode.Shared.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace LoginAuthenticatorCode.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly IPermissionRepository _permissionRepository;
        private readonly AppSetting _appSetting;

        public AuthenticationService(IHttpContextAccessor httpContext, IPermissionRepository permissionRepository, IOptions<AppSetting> appSetting)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _permissionRepository = permissionRepository ?? throw new ArgumentNullException(nameof(permissionRepository));
            _appSetting = appSetting.Value ?? throw new ArgumentNullException(nameof(appSetting));
        }

        public async Task<string> AuthenticateUserAsync(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));

            var claims = await GetAllRolesAsync(user);

            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new ClaimsPrincipal(identity);

            var token = Common.GenerateJwtToken(claims, _appSetting);

            return token;
        }

        public async Task<List<Claim>> GetAllRolesAsync(User user)
        {
            _ = user ?? throw new ArgumentNullException(nameof(user));

            var permission = await _permissionRepository.GetByIdAsync(user.PermissionId);

            var claims = new List<Claim>
            {
                  new Claim(ClaimTypes.Name, user.Name.ToString()),
                  new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                  new Claim(ClaimTypes.Role, permission.TypeAccess.ToString() ?? "Sem autorização"),
                  new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                  //new Claim(CustomClaimTypes.ClientId, (user.ClientId > 0 ? user.ClientId.ToString() : "0")),
                  new Claim(CustomClaimTypes.PermissionId, user.PermissionId.ToString())
            };
            return claims;
        }

        public User GetCurrentUser()
        {
            if (_httpContext.HttpContext is not null
             && _httpContext.HttpContext.User is not null
             && _httpContext.HttpContext.User.Identity is not null
             && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContext.HttpContext.User.Claims;

                var user = new User
                {
                    Id = Convert.ToInt64(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                    //ClientId = Convert.ToInt64(claims.FirstOrDefault(c => c.Type == CustomClaimTypes.ClientId)?.Value),
                    PermissionId = Convert.ToInt64(claims.FirstOrDefault(c => c.Type == CustomClaimTypes.PermissionId)?.Value),
                    Permission = new Permission
                    {
                        Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value,
                    },
                    Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                };
                return user;
            }
            else
                return new();
        }

        public bool IsAdmin()
        {
            if (_httpContext.HttpContext is not null
              && _httpContext.HttpContext.User is not null
              && _httpContext.HttpContext.User.Identity is not null
              && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var vclaims = _httpContext.HttpContext.User.Claims;
                var Roles = vclaims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (Roles is not null && Roles.Contains("Admin"))
                    return true;
                else
                    return false;
            }
            return false;
        }

        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        public long UserClient()
        {
            if (_httpContext.HttpContext is not null
           && _httpContext.HttpContext.User is not null
           && _httpContext.HttpContext.User.Identity is not null
           && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var client = claims.FirstOrDefault(c => c.Type == CustomClaimTypes.ClientId)?.Value;
                if (!string.IsNullOrEmpty(client))
                {
                    string vClient = client.Split(' ')[0];
                    return long.Parse(vClient);
                }
            }
            return 0;
        }

        public long UserId()
        {
            if (_httpContext.HttpContext is not null
            && _httpContext.HttpContext.User is not null
            && _httpContext.HttpContext.User.Identity is not null
            && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrEmpty(id))
                {
                    return long.Parse(id);
                }
            }
            return 0;
        }

        public string UserName()
        {
            if (_httpContext.HttpContext is not null
                && _httpContext.HttpContext.User is not null
                && _httpContext.HttpContext.User.Identity is not null
                && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                if (!string.IsNullOrEmpty(name))
                {
                    string vName = name.Split(' ')[0];
                    return vName;
                }
            }
            return string.Empty;
        }

        public string UserRole()
        {
            if (_httpContext.HttpContext is not null
             && _httpContext.HttpContext.User is not null
             && _httpContext.HttpContext.User.Identity is not null
             && _httpContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var claims = _httpContext.HttpContext.User.Claims;
                var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (!string.IsNullOrEmpty(role))
                {
                    string vRole = role.Split(' ')[0];
                    return vRole;
                }
            }
            return string.Empty;
        }
    }
}