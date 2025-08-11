using LoginAuthenticatorCode.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginAuthenticatorCode.Domain.Entities.Base;

    public class EntityBase
    {
        public long Id { get; set; }
        public virtual Situation Situation { get; set; } = Situation.Active;
        public virtual DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public virtual DateTime DateModified { get; set; }
        public virtual DateTime? DateUpdated { get; set; }
        public virtual DateTime? DateDeleted { get; set; }
        public long? UserIdCreated { get; set; }
        public long? UserIdModified { get; set; }
        public long? UserIdDeleted { get; set; }

}

