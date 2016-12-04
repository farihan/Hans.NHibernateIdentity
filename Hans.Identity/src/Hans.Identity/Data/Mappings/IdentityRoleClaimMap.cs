﻿using FluentNHibernate.Mapping;
using Hans.Identity.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Mappings
{
    public class IdentityRoleClaimMap : ClassMap<IdentityRoleClaim>
    {
        public IdentityRoleClaimMap()
        {
            Table("AspNetRoleClaims");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.ClaimType).Column("ClaimType");
            Map(x => x.ClaimValue).Column("ClaimValue");
            References(x => x.Role).Column("RoleId");
        }
    }
}
