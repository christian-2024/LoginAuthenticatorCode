using LoginAuthenticatorCode.Domain.Entities.Dtos.Base;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.UserDto.List;

    public class UserRequestListDto : BaseRequestListDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
}

