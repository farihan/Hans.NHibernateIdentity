using FluentNHibernate.Mapping;
using Hans.Identity.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Mappings
{
    public class IdentityUserLoginMap : ClassMap<IdentityUserLogin>
    {
        public IdentityUserLoginMap()
        {
            Table("AspNetUserLogins");
            LazyLoad();
            CompositeId().KeyProperty(x => x.LoginProvider, "LoginProvider")
                         .KeyProperty(x => x.ProviderKey, "ProviderKey");
            Map(x => x.ProviderDisplayName).Column("ProviderDisplayName");
            References(x => x.User).Column("UserId");
        }
    }
}
