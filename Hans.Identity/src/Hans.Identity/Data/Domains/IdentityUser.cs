using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Domains
{
    public class IdentityUser : IUser<string>
    {
        public virtual string Id { get; set; }
        public virtual int AccessFailedCount { get; set; }
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual string NormalizedEmail { get; set; }
        public virtual string NormalizedUserName { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual string UserName { get; set; }
        public virtual ICollection<IdentityUserRole> Roles { get; set; }
        public virtual ICollection<IdentityUserClaim> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin> Logins { get; set; }

        public IdentityUser()
        {
            Id = Guid.NewGuid().ToString();

            Roles = new List<IdentityUserRole>();
            Claims = new List<IdentityUserClaim>();
            Logins = new List<IdentityUserLogin>();
        }

        public IdentityUser(string userName) : this()
        {
            UserName = userName;
        }
    }
}
