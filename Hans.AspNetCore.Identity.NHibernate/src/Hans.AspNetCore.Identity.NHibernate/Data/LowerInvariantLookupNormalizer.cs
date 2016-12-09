using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data
{
    public class LowerInvariantLookupNormalizer : ILookupNormalizer
    {
        public string Normalize(string key)
        {
            return key.ToLowerInvariant();
        }
    }
}
