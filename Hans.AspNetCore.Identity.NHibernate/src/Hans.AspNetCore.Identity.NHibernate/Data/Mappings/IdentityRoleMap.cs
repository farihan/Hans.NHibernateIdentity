using FluentNHibernate.Mapping;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Mappings
{
    public class IdentityRoleMap : ClassMap<IdentityRole>
    {
        public IdentityRoleMap()
        {
            Table("AspNetRoles");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned().Column("Id");
            Map(x => x.ConcurrencyStamp).Column("ConcurrencyStamp");
            Map(x => x.Name)
                .Length(255)
                .Not.Nullable()
                .Unique()
                .Column("Name");
            Map(x => x.NormalizedName).Column("NormalizedName")
                .Index("RoleNameIndex");
            HasMany(x => x.Users)
                .Table("AspNetUserRoles").Inverse()
                .AsBag()
                .KeyColumn("RoleId")
                .Cascade.AllDeleteOrphan();
            HasMany(x => x.Claims)
                .Table("AspNetRoleClaims")
                .AsBag()
                .KeyColumn("RoleId")
                .Cascade.AllDeleteOrphan();
        }
    }
}
