namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;

public record class UserRequestInsertDto(string Name,
                                         string Password,
                                         string Email);