using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Domains
{
    public class IdentityUserLogin
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual string ProviderDisplayName { get; set; }
        public virtual IdentityUser User { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj as IdentityUserLogin;
            if (t == null) return false;
            if (LoginProvider == t.LoginProvider
             && ProviderKey == t.ProviderKey)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            int hash = GetType().GetHashCode();
            hash = (hash * 397) ^ LoginProvider.GetHashCode();
            hash = (hash * 397) ^ ProviderKey.GetHashCode();

            return hash;
        }
    }
}
