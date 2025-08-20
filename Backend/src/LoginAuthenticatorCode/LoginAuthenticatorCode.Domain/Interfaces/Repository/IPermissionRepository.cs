using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Repository.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Repository;

public interface IPermissionRepository : IRepositoryBase<Permission>
{
    Task<PagedListDto<Permission>> GetAllPermissionsByFilterAsync(PermissionRequestListDto requestDto);

    Task<PagedListDto<Permission>> GetAllPermissionsAdminAsync(PermissionRequestListDto requestDto);

    Task<PagedListDto<Permission>> GetAllPermissionsClientAsync(PermissionRequestListDto requestDto);
}