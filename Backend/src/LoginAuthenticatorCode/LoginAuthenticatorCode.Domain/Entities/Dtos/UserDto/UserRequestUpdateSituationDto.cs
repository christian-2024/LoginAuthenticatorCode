using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;

public record UserRequestUpdateSituationDto
{
    public virtual Situation Situation { get; set; }
}