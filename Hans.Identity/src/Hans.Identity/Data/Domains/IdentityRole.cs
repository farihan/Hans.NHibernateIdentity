using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Domains
{
    public class IdentityRole : IRole<string>
    {
        public virtual string Id { get; set; }
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual ICollection<IdentityUserRole> Users { get; set; }
        public virtual ICollection<IdentityRoleClaim> Claims { get; set; }

        public IdentityRole()
        {
            Id = Guid.NewGuid().ToString();

            Users = new List<IdentityUserRole>();
            Claims = new List<IdentityRoleClaim>();
        }

        public IdentityRole(string roleName) : this()
        {
            Name = roleName;
        }
    }
}
