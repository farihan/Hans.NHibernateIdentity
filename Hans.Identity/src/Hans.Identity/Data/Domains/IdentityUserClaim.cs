using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Domains
{
    public class IdentityUserClaim
    {
        public virtual int Id { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
