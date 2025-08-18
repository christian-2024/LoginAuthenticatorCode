using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities.Dtos.Base;

    public class BaseRequestListDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public Situation Situation { get; set; } = Situation.Active;
}

