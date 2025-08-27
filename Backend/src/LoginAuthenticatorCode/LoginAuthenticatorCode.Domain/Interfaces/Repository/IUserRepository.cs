using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Repository.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Repository;

    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<PagedListDto<User>> GetAllUserByFilterAsync(UserRequestListDto requestDto);
    }

