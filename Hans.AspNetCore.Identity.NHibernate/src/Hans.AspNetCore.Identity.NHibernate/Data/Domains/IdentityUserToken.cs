using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Domains
{
    public class IdentityUserToken
    {
        public virtual string UserId { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as IdentityUserToken;
            if (t == null) return false;
            if (UserId == t.UserId
             && LoginProvider == t.LoginProvider
             && Name == t.Name)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ UserId.GetHashCode();
            hash = (hash * 397) ^ LoginProvider.GetHashCode();
            hash = (hash * 397) ^ Name.GetHashCode();

            return hash;
        }
    }
}
