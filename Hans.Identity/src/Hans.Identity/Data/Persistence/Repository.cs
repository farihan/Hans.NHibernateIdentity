using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hans.Identity.Data.Persistence
{
    public class Repository<TDomain> : IRepository<TDomain>, IDisposable where TDomain : class
    {
        private readonly ISession session;

        public Repository(ISession session)
        {
            this.session = session;
        }

        public void Save(TDomain instance)
        {
            using (var tx = session.BeginTransaction())
            {
                session.Save(instance);

                try
                {
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void Update(TDomain instance)
        {
            using (var tx = session.BeginTransaction())
            {
                session.Update(instance);

                try
                {
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public void Delete(TDomain instance)
        {
            using (var tx = session.BeginTransaction())
            {
                session.Delete(instance);

                try
                {
                    tx.Commit();
                }
                catch (Exception)
                {
                    tx.Rollback();
                    throw;
                }
            }
        }

        public IQueryable<TDomain> FindAll()
        {
            return session.Query<TDomain>();
        }

        public IQueryable<TDomain> FindAllBy(Expression<Func<TDomain, bool>> where)
        {
            return session.Query<TDomain>().Where(where);
        }

        public TDomain FindOneBy(Expression<Func<TDomain, bool>> where)
        {
            return session.Query<TDomain>().Where(where).SingleOrDefault();
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}
