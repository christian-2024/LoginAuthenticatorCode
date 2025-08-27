using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Interfaces.Repository.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Repository;

public interface INotificationRepository : IRepositoryBase<Notification>
{
    Task<PagedListDto<Notification>> GetAllNotificationsByFilterAsync(NotificationRequestListDto requestDto);
}