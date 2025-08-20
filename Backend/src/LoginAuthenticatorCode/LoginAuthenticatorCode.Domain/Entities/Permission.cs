using LoginAuthenticatorCode.Domain.Entities.Base;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities;

public class Permission : EntityBase
{
    public string Name { get; set; }
    public virtual TypeAcess TypeAccess { get; set; }
    public string Description { get; set; }
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}