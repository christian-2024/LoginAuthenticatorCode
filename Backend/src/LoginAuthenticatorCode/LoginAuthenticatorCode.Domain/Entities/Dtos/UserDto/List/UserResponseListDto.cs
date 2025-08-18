namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;

public class UserResponseListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Cpf { get; set; }
}