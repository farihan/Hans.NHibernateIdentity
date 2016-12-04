using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Hans.Identity.Data;
using Hans.Identity.Data.Domains;
using Hans.Identity.Data.Persistence;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Identity.Tests
{
    public abstract class BaseTest
    {
        protected string Connection { get; set; }

        protected ISession Session { get; set; }

        protected BaseTest()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile("appsettings.json");

            var configuration = configurationBuilder.Build();
            Connection = configuration["ConnectionStrings:Development"];

            Session = new PersistenceConfiguration().Initialize(Connection).OpenSession();
        }
        
        protected void CreateDatabase(string connection)
        {
            var configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connection).ShowSql)
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Hans.Identity")))
                .BuildConfiguration();

            var exporter = new SchemaExport(configuration);

            exporter.Drop(true, true);
            exporter.Create(true, true);
        }

        protected RoleManager<IdentityRole> GetRoleManager(ISession session)
        {
            var store = new RoleStore<IdentityRole>(session);

            return new RoleManager<IdentityRole>(store);
        }

        protected UserManager<IdentityUser> GetUserManager(ISession session)
        {
            var store = new UserStore<IdentityUser>(session);

            return new UserManager<IdentityUser>(store);
        }
    }
}
