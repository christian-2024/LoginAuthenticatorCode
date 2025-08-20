using LoginAuthenticatorCode.Domain.Entities.Base;
using LoginAuthenticatorCode.Domain.Enum;

namespace LoginAuthenticatorCode.Domain.Entities;

public class Notification : EntityBase
{
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsRead { get; set; }
    public virtual TypeNotification TypeNotification { get; set; }

    /// <summary>
    /// Relacionamentos
    /// </summary>
    ///
    public long UserId { get; set; }

    public virtual User User { get; set; }
}