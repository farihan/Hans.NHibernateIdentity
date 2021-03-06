﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Domains
{
    public class IdentityRoleClaim
    {
        public virtual int Id { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}
