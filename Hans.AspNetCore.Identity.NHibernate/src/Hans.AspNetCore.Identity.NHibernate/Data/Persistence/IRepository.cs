using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data.Persistence
{

    public interface IRepository<TDomain> where TDomain : class
    {
        void Save(TDomain instance);
        void Update(TDomain instance);
        void Delete(TDomain instance);
        IQueryable<TDomain> FindAll();
        IQueryable<TDomain> FindAllBy(Expression<Func<TDomain, bool>> where);
        TDomain FindOneBy(Expression<Func<TDomain, bool>> where);
    }
}
