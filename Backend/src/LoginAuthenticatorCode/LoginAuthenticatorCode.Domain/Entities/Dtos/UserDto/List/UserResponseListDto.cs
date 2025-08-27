using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;

public class UserResponseListDto
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Cpf { get; set; }
    public long PermissionId { get; set; }
    public PermissionResponseListDto Permission { get; set; }
}