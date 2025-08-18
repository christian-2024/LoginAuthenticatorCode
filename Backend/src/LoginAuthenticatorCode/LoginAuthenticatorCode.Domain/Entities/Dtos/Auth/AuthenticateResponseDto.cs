using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.Auth;

public class AuthenticateResponseDto
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Login { get; set; }
    public string Token { get; set; }
    public Perfil? Perfil { get; set; }
}