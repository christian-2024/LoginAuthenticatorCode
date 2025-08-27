using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Interfaces.Service.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Service;

public interface INotificationService : IServiceBase<Notification>
{
    Task<PagedListDto<NotificationResponseListDto>> GetAllNotificationsByFilterAsync(NotificationRequestListDto requestDto);
}