using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Domains
{
    public class IdentityUserRole
    {
        public virtual string UserId { get; set; }
        public virtual string RoleId { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual IdentityRole Role { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as IdentityUserRole;
            if (t == null) return false;
            if (UserId == t.UserId
             && RoleId == t.RoleId)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();
            hash = (hash * 397) ^ RoleId.GetHashCode();

            return hash;
        }
    }
}
