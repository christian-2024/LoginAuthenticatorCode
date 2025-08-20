using LoginAuthenticatorCode.Data.Context;
using LoginAuthenticatorCode.Data.Repository.Base;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto.Helper;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
using LoginAuthenticatorCode.Domain.Enum;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LoginAuthenticatorCode.Data.Repository;

public class PermissionRepository : RepositoryBase<Permission>, IPermissionRepository
{
    public PermissionRepository(LoginAuthenticatorContext loginAuthenticatorContext) : base(loginAuthenticatorContext) { }

    public async Task<PagedListDto<Permission>> GetAllPermissionsAdminAsync(PermissionRequestListDto requestDto) { 
        
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        IQueryable<Permission> query = _loginAuthenticatorContext.Permission.AsNoTracking()
            .Where(c => c.TypeAccess == TypeAcess.Attendant || c.TypeAccess == TypeAcess.Financial);

        if (requestDto is { })
        {
            if (requestDto.Situation.GetHashCode() > 0)
                query = query.Where(c => c.Situation == requestDto.Situation);

            if (requestDto.TypeAcess.GetHashCode() > 0)
                query = query.Where(c => c.TypeAccess == requestDto.TypeAcess);

            if (!string.IsNullOrEmpty(requestDto.Name))
                query = query.Where(c => c.Name.ToLower().Trim() == requestDto.Name.ToLower().Trim());
        }

        var result = query.Select( c => new Permission
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            TypeAccess = c.TypeAccess,
            DateCreated = c.DateCreated,
            DateModified = c.DateModified,
        });
        return await PaginationHelperDto.CreateAsync(result, requestDto!.PageNumber, requestDto!.PageSize);
    }

    public Task<PagedListDto<Permission>> GetAllPermissionsByFilterAsync(PermissionRequestListDto requestDto)
    {
        throw new NotImplementedException();
    }

    public Task<PagedListDto<Permission>> GetAllPermissionsClientAsync(PermissionRequestListDto requestDto)
    {
        throw new NotImplementedException();
    }
}

