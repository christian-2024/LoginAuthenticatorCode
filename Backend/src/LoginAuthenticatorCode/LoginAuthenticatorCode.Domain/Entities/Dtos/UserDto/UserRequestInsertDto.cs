namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;

public record class UserRequestInsertDto
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public string Cpf { get; set; }
    public long PermissionId { get; set; }
}