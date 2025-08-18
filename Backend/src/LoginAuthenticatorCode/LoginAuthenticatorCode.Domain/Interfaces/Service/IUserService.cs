using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PaginationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using LoginAuthenticatorCode.Domain.Interfaces.Service.Base;

namespace LoginAuthenticatorCode.Domain.Interfaces.Service;

public interface IUserService : IServiceBase<User>
{
    Task<PagedListDto<UserResponseListDto>> GetAllUsersByFilterAsync(UserRequestListDto requestDto);

    //Task<AuthenticateResponseDto> LoginAsync(AuthenticateRequestDto requestDto);

    string GenerateNewHashPassword(string password);

    bool VerifyHashPassword(string password, string currentPassword);
}