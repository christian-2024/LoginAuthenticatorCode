namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;

public class UserResponseFormDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Cpf { get; set; }
    public long PermissionId { get; set; }
    public List<string>? Errors { get; set; } = new List<string>();
}