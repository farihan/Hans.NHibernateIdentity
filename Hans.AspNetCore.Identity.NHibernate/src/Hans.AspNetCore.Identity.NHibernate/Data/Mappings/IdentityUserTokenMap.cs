﻿using FluentNHibernate.Mapping;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Mappings
{
    public class IdentityUserTokenMap : ClassMap<IdentityUserToken>
    {
        public IdentityUserTokenMap()
        {
            Table("AspNetUserTokens");
            LazyLoad();
            CompositeId().KeyProperty(x => x.UserId, "UserId")
                         .KeyProperty(x => x.LoginProvider, "LoginProvider")
                         .KeyProperty(x => x.Name, "Name");
            Map(x => x.Value).Column("Value");
        }
    }
}
