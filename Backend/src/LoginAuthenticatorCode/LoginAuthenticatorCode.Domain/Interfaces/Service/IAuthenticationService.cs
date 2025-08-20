using LoginAuthenticatorCode.Domain.Entities;
using System.Security.Claims;

namespace LoginAuthenticatorCode.Domain.Interfaces.Service;

public interface IAuthenticationService
{
    Task<string> AuthenticateUserAsync(User user);

    Task<List<Claim>> GetAllRolesAsync(User user);

    Task LogoutAsync();

    User GetCurrentUser();

    bool IsAdmin();

    string UserName();

    string UserRole();

    long UserClient();

    long UserId();
}