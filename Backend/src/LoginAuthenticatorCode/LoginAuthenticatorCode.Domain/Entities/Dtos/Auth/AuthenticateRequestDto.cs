using System.ComponentModel.DataAnnotations;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.Auth;

public class AuthenticateRequestDto
{
    [Required]
    public string Login { get; set; }

    [Required]
    public string Password { get; set; }

    public List<string>? Errors { get; set; } = new List<string>();
}