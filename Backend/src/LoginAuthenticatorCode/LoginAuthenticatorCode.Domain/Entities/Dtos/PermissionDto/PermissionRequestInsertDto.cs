using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto;

public record PermissionRequestInsertDto
{
    public string Description { get; set; }
    public virtual TypeAcess TypeAccess { get; set; }
}