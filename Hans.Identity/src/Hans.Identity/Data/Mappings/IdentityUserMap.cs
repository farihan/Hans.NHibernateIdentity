using FluentNHibernate.Mapping;
using Hans.Identity.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Mappings
{
    public class IdentityUserMap : ClassMap<IdentityUser>
    {
        public IdentityUserMap()
        {
            Table("AspNetUsers");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.AccessFailedCount).Column("AccessFailedCount").Not.Nullable();
            Map(x => x.ConcurrencyStamp).Column("ConcurrencyStamp");
            Map(x => x.Email).Column("Email");
            Map(x => x.EmailConfirmed).Column("EmailConfirmed").Not.Nullable();
            Map(x => x.LockoutEnabled).Column("LockoutEnabled").Not.Nullable();
            Map(x => x.LockoutEnd).Column("LockoutEnd");
            Map(x => x.NormalizedEmail).Column("NormalizedEmail");
            Map(x => x.NormalizedUserName).Column("NormalizedUserName");
            Map(x => x.PasswordHash).Column("PasswordHash");
            Map(x => x.PhoneNumber).Column("PhoneNumber");
            Map(x => x.PhoneNumberConfirmed).Column("PhoneNumberConfirmed").Not.Nullable();
            Map(x => x.SecurityStamp).Column("SecurityStamp");
            Map(x => x.TwoFactorEnabled).Column("TwoFactorEnabled").Not.Nullable();
            Map(x => x.UserName)
                .Length(255)
                .Not.Nullable()
                .Unique()
                .Column("UserName");
            HasMany(x => x.Roles).Inverse()
                .Table("AspNetUserRoles")
                .AsBag()
                .KeyColumn("UserId")
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.Claims)
                .Table("AspNetUserClaims")
                .AsBag()
                .KeyColumn("UserId")
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.Logins)
                .Table("AspNetUserLogins")
                .AsBag()
                .KeyColumn("UserId")
                .Cascade.AllDeleteOrphan();
        }
    }
}
