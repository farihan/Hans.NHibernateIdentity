using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hans.Identity.Data
{
    public class PersistenceConfiguration
    {
        public NHibernate.ISessionFactory Initialize(string connection)
        {
            var sf = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(connection)
                    .Raw("prepare_sql", "true")
                    .Raw("cache.use_query_cache", "true")
                    .Raw("cache.use_second_level_cache", "true")
                    .DoNot
                    .ShowSql())
                .ExposeConfiguration(c => c.SetProperty("current_session_context_class", "web"))
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Hans.Identity")))
                .BuildSessionFactory();

            return sf;
        }
    }
}
