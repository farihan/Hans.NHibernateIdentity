using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Hans.AspNetCore.Identity.NHibernate.Data.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Data
{
    public static class NHibernateIdentityBuildExtensions
    {
        public static IdentityBuilder AddNHibernateIdentity<TUser, TRole>(this IServiceCollection services, string connection)
            where TUser : IdentityUser
            where TRole : IdentityRole
        {
            services.AddSingleton(x => new PersistenceConfiguration().Initialize(connection));
            services.AddTransient(x => x.GetService<ISessionFactory>().OpenSession());

            services.AddTransient<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
            services.AddTransient<ILookupNormalizer, LowerInvariantLookupNormalizer>();
            services.AddTransient<IdentityErrorDescriber>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();
            
            services.AddTransient<IUserStore<IdentityUser>, UserStore<IdentityUser>>();
            services.AddTransient<IRoleStore<IdentityRole>, RoleStore<IdentityRole>>();

            services.AddTransient<UserManager<IdentityUser>>();
            services.AddTransient<RoleManager<IdentityRole>>();
            services.AddTransient<SignInManager<IdentityUser>>();

            return new IdentityBuilder(typeof(TUser), typeof(TRole), services);
        }

        public static IApplicationBuilder UseNHibernateIdentity(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            var options = app.ApplicationServices.GetRequiredService<IOptions<IdentityOptions>>().Value;
            app.UseCookieAuthentication(options.Cookies.ExternalCookie);
            app.UseCookieAuthentication(options.Cookies.TwoFactorRememberMeCookie);
            app.UseCookieAuthentication(options.Cookies.TwoFactorUserIdCookie);
            app.UseCookieAuthentication(options.Cookies.ApplicationCookie);
            return app;
        }
    }
}
