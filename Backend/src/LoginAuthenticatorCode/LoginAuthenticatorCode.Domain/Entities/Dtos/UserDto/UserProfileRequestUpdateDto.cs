namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;

public record UserProfileRequestUpdateDto
{
    public string NewPassword { get; set; }
    public string CurrentPassword { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Cpf { get; set; }
    public string PhoneNumber { get; set; }
}