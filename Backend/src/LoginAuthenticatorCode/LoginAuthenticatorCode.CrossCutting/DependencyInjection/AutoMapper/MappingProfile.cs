using AutoMapper;
using LoginAuthenticatorCode.Domain.Entities;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.NotificationDto.List;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto;
using LoginAuthenticatorCode.Domain.Entities.Dtos.PermissionDto.List;
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

        #region Notification
        CreateMap<Notification, NotificationRequestListDto>().ReverseMap();
        CreateMap<Notification, NotificationRequestListDto>().ReverseMap();
        CreateMap<Notification, NotificationRequestInsertDto>().ReverseMap();
        CreateMap<Notification, NotificationRequestUpdateDto>().ReverseMap();
        #endregion

        #region Permission
        CreateMap<Permission, PermissionRequestListDto>().ReverseMap();
        CreateMap<Permission, PermissionResponseListDto>().ReverseMap();
        CreateMap<Permission, PermissionRequestInsertDto>().ReverseMap();
        CreateMap<Permission, PermissionRequestUpdateDto>().ReverseMap(); 
        #endregion
    }
}