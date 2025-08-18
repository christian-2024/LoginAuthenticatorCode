using LoginAuthenticatorCode.Domain.Entities.Base;

namespace LoginAuthenticatorCode.Domain.Entities;

    public class User : EntityBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Cpf { get; set; }

}

