using LoginAuthenticatorCode.Data.Context;
using LoginAuthenticatorCode.Data.Repository.Base;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using LoginAuthenticatorCode.Domain.Enum;
using LoginAuthenticatorCode.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace LoginAuthenticatorCode.Data.Repository;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(LoginAuthenticatorContext loginAuthenticatorContext) : base(loginAuthenticatorContext)
    {
    }

    public async Task<PagedListDto<User>> GetAllUserByFilterAsync(UserRequestListDto requestDto)
    {
        _ = requestDto ?? throw new ArgumentNullException(nameof(requestDto));

        IQueryable<User> query = _loginAuthenticatorContext.User.AsNoTracking().OrderBy(c => c.Name).Where(c => c.Situation != Situation.Deleted);

        if (requestDto is { })
        {
            if (requestDto.Situation.GetHashCode() > 0)
                query = query.Where(c => c.Situation == requestDto.Situation);

            if (!string.IsNullOrWhiteSpace(requestDto.Name))
                query = query.Where(c => c.Name.ToLower().Contains(requestDto.Name.ToLower()));

            if (!string.IsNullOrWhiteSpace(requestDto.Email))
                query = query.Where(c => c.Email.ToLower().Contains(requestDto.Email.ToLower()));
        }
        return await PagedListDto<User>.ToPagedListAsync(query, requestDto.PageNumber, requestDto.PageSize);
    }
}