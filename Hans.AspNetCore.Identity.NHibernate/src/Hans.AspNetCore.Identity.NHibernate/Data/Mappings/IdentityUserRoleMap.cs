﻿using FluentNHibernate.Mapping;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Mappings
{
    public class IdentityUserRoleMap : ClassMap<IdentityUserRole>
    {
        public IdentityUserRoleMap()
        {
            Table("AspNetUserRoles");
            LazyLoad();
            CompositeId().KeyProperty(x => x.UserId, "UserId")
                         .KeyProperty(x => x.RoleId, "RoleId");
            References(x => x.User).Column("UserId")
               .Not.Insert()
               .Not.Update();
            References(x => x.Role).Column("RoleId")
               .Not.Insert()
               .Not.Update();
        }
    }
}
