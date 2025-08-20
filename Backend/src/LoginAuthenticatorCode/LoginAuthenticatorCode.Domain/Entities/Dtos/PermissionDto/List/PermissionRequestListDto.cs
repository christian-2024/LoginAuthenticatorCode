using LoginAuthenticatorCode.Domain.Entities.Dtos.Base;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;

public class PermissionRequestListDto : BaseRequestListDto
{
    public string Name { get; set; }
    public virtual TypeAcess TypeAcess { get; set; }
}