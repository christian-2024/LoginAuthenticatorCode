using AutoMapper;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;
using Microsoft.Identity.Client;

namespace LoginAuthenticatorCode.CrossCutting.DependencyInjection.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        #region User
        CreateMap<User, UserResponseListDto>().ReverseMap();
        CreateMap<User, UserRequestListDto>().ReverseMap();
        CreateMap<User, UserRequestInsertDto>().ReverseMap();
        CreateMap<User, UserRequestUpdateDto>().ReverseMap();
        CreateMap<User, UserFormResponseDto>().ReverseMap(); 
        #endregion


    }
}