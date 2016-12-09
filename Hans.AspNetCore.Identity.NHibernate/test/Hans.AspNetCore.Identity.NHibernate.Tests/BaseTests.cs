using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Hans.AspNetCore.Identity.NHibernate.Data;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Hans.AspNetCore.Identity.NHibernate.Data.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    public class BaseTests
    {
        protected readonly string PASSWORD = "User@123456";
        protected string Connection { get; set; }
        protected ISession Session { get; set; }

        protected BaseTests()
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
                .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.Load("Hans.AspNetCore.Identity.NHibernate")))
                .BuildConfiguration();

            var exporter = new SchemaExport(configuration);

            exporter.Drop(true, true);
            exporter.Create(true, true);
        }

        protected RoleManager<IdentityRole> GetRoleManager(ISession session)
        {
            var store = new RoleStore<IdentityRole>(session);
            var validator = new List<RoleValidator<IdentityRole>>();
            var keyNormalizer = new LowerInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();
            var logger = new Microsoft.Extensions.Logging.Logger<RoleManager<IdentityRole>>(new Microsoft.Extensions.Logging.LoggerFactory());

            var context = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);

            return new RoleManager<IdentityRole>(store, validator, keyNormalizer, errors, logger, contextAccessor.Object);
        }

        protected UserManager<IdentityUser> GetUserManager(ISession session)
        {
            var store = new UserStore<IdentityUser>(session);

            var identityOptions = new IdentityOptions();
            var options = new Mock<IOptions<IdentityOptions>>();
            options.Setup(x => x.Value).Returns(identityOptions);

            var passwordHasher = new PasswordHasher<IdentityUser>();
            var validator = new List<UserValidator<IdentityUser>>();
            var passwordValidator = new List<PasswordValidator<IdentityUser>>();
            var keyNormalizer = new LowerInvariantLookupNormalizer();
            var errors = new IdentityErrorDescriber();

            var services = new ServiceCollection();
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddLogging();
            var providers = services.BuildServiceProvider();

            var logger = new Microsoft.Extensions.Logging.Logger<UserManager<IdentityUser>>(new Microsoft.Extensions.Logging.LoggerFactory());

            return new UserManager<IdentityUser>(store, options.Object, passwordHasher, validator, passwordValidator, keyNormalizer, errors, providers, logger);
        }

        protected SignInManager<IdentityUser> GetSignInManager(ISession session)
        {
            var userManager = GetUserManager(session);
            var roleManager = GetRoleManager(session);
            
            var context = new Mock<Microsoft.AspNetCore.Http.HttpContext>();
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            contextAccessor.Setup(x => x.HttpContext).Returns(context.Object);

            var identityOptions = new IdentityOptions();
            identityOptions.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromMinutes(10);
            identityOptions.Cookies.ApplicationCookie.SlidingExpiration = false;

            var options = new Mock<IOptions<IdentityOptions>>();            
            options.Setup(x => x.Value).Returns(identityOptions);

            var claims = new UserClaimsPrincipalFactory<IdentityUser, IdentityRole>(userManager, roleManager, options.Object);
            var logger = new Microsoft.Extensions.Logging.Logger<SignInManager<IdentityUser>>(new Microsoft.Extensions.Logging.LoggerFactory());
            
            return new SignInManager<IdentityUser>(userManager, contextAccessor.Object, claims, options.Object, logger);
        }
    }
}
