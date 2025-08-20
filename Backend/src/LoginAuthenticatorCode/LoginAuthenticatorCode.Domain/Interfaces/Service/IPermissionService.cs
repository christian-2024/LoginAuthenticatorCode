using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Service.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Service;

public interface IPermissionService : IServiceBase<Permission>
{
    Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsByFilterAsync(PermissionRequestListDto requestDto);

    Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsAdminAsync(PermissionRequestListDto requestDto);

    Task<PagedListDto<PermissionResponseListDto>> GetAllPermissionsClientAsync(PermissionRequestListDto requestDto);
}