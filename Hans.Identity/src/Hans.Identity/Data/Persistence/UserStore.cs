using Hans.Identity.Data.Domains;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using NHibernate;
using System.Globalization;

namespace Hans.Identity.Data.Persistence
{
    public class UserStore<TDomain> : IUserStore<TDomain>,
        IUserPasswordStore<TDomain>,
        IUserRoleStore<TDomain>,
        IUserLoginStore<TDomain>,
        IUserSecurityStampStore<TDomain>,
        IUserEmailStore<TDomain>,
        IUserClaimStore<TDomain>,
        IUserPhoneNumberStore<TDomain>,
        IUserTwoFactorStore<TDomain, string>,
        IUserLockoutStore<TDomain, string>,
        IQueryableUserStore<TDomain> 
        where TDomain : IdentityUser
    {
        private readonly IRepository<TDomain> userRepository;
        private readonly IRepository<IdentityRole> roleRepository;

        public UserStore(ISession session)
        {
            userRepository = new Repository<TDomain>(session);
            roleRepository = new Repository<IdentityRole>(session);
        }

        public Task AddClaimAsync(TDomain user, Claim claim)
        {
            if(user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            user.Claims.Add(new IdentityUserClaim()
            {
                User = user,
                ClaimType = claim.Type,
                ClaimValue = claim.Value
            });

            return Task.FromResult(0);
        }

        public Task AddLoginAsync(TDomain user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            user.Logins.Add(new IdentityUserLogin()
            {
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider
            });

            userRepository.Save(user);

            return Task.FromResult(0);
        }

        public Task AddToRoleAsync(TDomain user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(roleName));
            }

            var identityRole = roleRepository.FindOneBy(x => x.Name.ToLower() == roleName.ToLower());

            if (identityRole == null)
            {
                throw new InvalidOperationException(string.Format("Role {0} does not exist", nameof(roleName)));
            }

            var userRoles = new IdentityUserRole()
            {
                UserId = user.Id,
                RoleId = identityRole.Id,
                Role = identityRole,
                User = user
            };

            user.Roles.Add(userRoles);

            return Task.FromResult(0);
        }

        public Task CreateAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            userRepository.Save(user);

            return Task.FromResult(0);
        }

        public Task DeleteAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            userRepository.Delete(user);

            return Task.FromResult(0);
        }

        public Task<TDomain> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var list = userRepository.FindAllBy(x => x.Logins
                .Any(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey))
                .Select(x => x);

            return Task.FromResult(list.FirstOrDefault());
        }

        public Task<TDomain> FindByEmailAsync(string email)
        {
            return Task.FromResult(userRepository.FindOneBy(x => x.Email.ToLower() == email.ToLower()));
        }

        public Task<TDomain> FindByIdAsync(string userId)
        {
            return Task.FromResult(userRepository.FindOneBy(x => x.Id.ToLower() == userId.ToLower()));
        }

        public Task<TDomain> FindByNameAsync(string userName)
        {
            return Task.FromResult(userRepository.FindOneBy(x => x.UserName.ToLower() == userName.ToLower()));
        }

        public Task<int> GetAccessFailedCountAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<IList<Claim>> GetClaimsAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var list = new List<Claim>();

            foreach(var identityClaim in user.Claims)
            {
                list.Add(new Claim(identityClaim.ClaimType, identityClaim.ClaimValue));
            }

            return Task.FromResult<IList<Claim>>(list);
        }

        public Task<string> GetEmailAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.LockoutEnd.Value);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var list = new List<UserLoginInfo>();
            foreach (var identityUserLogin in user.Logins)
            {
                list.Add(new UserLoginInfo(identityUserLogin.LoginProvider, identityUserLogin.ProviderKey));
            }

            return Task.FromResult<IList<UserLoginInfo>>(list);
        }

        public Task<string> GetPasswordHashAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task<IList<string>> GetRolesAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var list = user.Roles.Select(x => x.Role.Name).ToList();

            return Task.FromResult<IList<string>>(list);
        }

        public Task<string> GetSecurityStampAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.SecurityStamp);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<bool> HasPasswordAsync(TDomain user)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.AccessFailedCount = user.AccessFailedCount + 1;

            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> IsInRoleAsync(TDomain user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(roleName));
            }

            var role = user.Roles.Where(x => x.Role.Name.ToLower() == roleName.ToLower());

            return Task.FromResult(role != null);
        }

        public Task RemoveClaimAsync(TDomain user, Claim claim)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var claims = user.Claims.Where(x => x.ClaimValue == claim.Value).ToList();

            foreach (var identityUserClaim in claims)
            {
                user.Claims.Remove(identityUserClaim);
            }

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(TDomain user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(roleName));
            }

            var roles = user.Roles.Where(x => x.Role.Name.ToLower() == roleName.ToLower()).ToList();

            foreach (var role in roles)
            {
                user.Roles.Remove(role);
            }

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(TDomain user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (login == null)
            {
                throw new ArgumentNullException(nameof(login));
            }

            var info = user.Logins.FirstOrDefault(x => x.LoginProvider == login.LoginProvider && x.ProviderKey == login.ProviderKey);
            if (info != null)
            {
                user.Logins.Remove(info);
            }

            return Task.FromResult(0);
        }

        public Task ResetAccessFailedCountAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.AccessFailedCount = 0;

            return Task.FromResult(0);
        }

        public Task SetEmailAsync(TDomain user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Email = email;

            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(TDomain user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.EmailConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(TDomain user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LockoutEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(TDomain user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.LockoutEnd = lockoutEnd;

            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(TDomain user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(TDomain user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TDomain user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.PhoneNumberConfirmed = confirmed;

            return Task.FromResult(0);
        }

        public Task SetSecurityStampAsync(TDomain user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TDomain user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.TwoFactorEnabled = enabled;

            return Task.FromResult(0);
        }

        public Task UpdateAsync(TDomain user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            userRepository.Update(user);

            return Task.FromResult(0);
        }

        public IQueryable<TDomain> Users
        {
            get
            {
                return userRepository.FindAll();
            }
        }

        public void Dispose()
        {

        }
    }
}
