using LoginAuthenticatorCode.Domain.Entities.Dtos.Base;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;

public class NotificationRequestListDto : BaseRequestListDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; }
    public virtual TypeNotification TypeNotification { get; set; }
}