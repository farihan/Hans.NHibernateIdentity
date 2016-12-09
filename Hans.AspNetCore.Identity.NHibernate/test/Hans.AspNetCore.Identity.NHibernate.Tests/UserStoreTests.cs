using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Hans.AspNetCore.Identity.NHibernate.Data.Persistence;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Hans.AspNetCore.Identity.NHibernate.Data;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    [TestClass]
    public class UserStoreTests : BaseTests
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanUsers()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var user2 = new IdentityUser("User2")
            {
                Email = "user2@user.com",
                EmailConfirmed = true
            };

            var user3 = new IdentityUser("User3")
            {
                Email = "user3@user.com",
                EmailConfirmed = true
            };

            // act
            store.CreateAsync(user1);
            store.CreateAsync(user2);
            store.CreateAsync(user3);

            // assert
            Assert.AreEqual(3, store.Users.Count());
        }

        [TestMethod]
        public void CanAddGetClaimsAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var list = new List<Claim>();
            var claim1 = new Claim("ClaimType1", "ClaimValue1", "ClaimValueType1");

            list.Add(claim1);

            // act
            store.AddClaimsAsync(user1, list);

            var r = store.GetClaimsAsync(user1);

            // assert
            Assert.AreEqual(1, r.Result.Count());
        }

        [TestMethod]
        public void CanAddGetLoginAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var login1 = new UserLoginInfo("LoginProvider1", "ProviderKey1", "DisplayNam1");

            // act
            store.AddLoginAsync(user1, login1);

            var r = store.GetLoginsAsync(user1);

            // assert
            Assert.AreEqual(1, r.Result.Count());
        }

        [TestMethod]
        public void CanAddGetRoleAysnc()
        {
            var roleStore = new RoleStore<IdentityRole>(Session);
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            roleStore.CreateAsync(role1);

            store.AddToRoleAsync(user1, "Role1");

            var r = store.GetRolesAsync(user1);

            // assert
            Assert.AreEqual(1, r.Result.Count());
        }

        [TestMethod]
        public void CanCreateUpdateDeleteUser()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.CreateAsync(user1);

            var r1 = store.FindByIdAsync(user1.Id);
            r1.Result.Email = "user1updated@user.com";

            // assert
            Assert.AreEqual("User1", r1.Result.UserName);

            store.UpdateAsync(r1.Result);

            var r2 = store.FindByIdAsync(user1.Id);

            // assert
            Assert.AreEqual("user1updated@user.com", r1.Result.Email);

            store.DeleteAsync(r1.Result);

            var r3 = store.FindByIdAsync(user1.Id);

            // assert
            Assert.IsNull(r3.Result);
        }

        [TestMethod]
        public void CanFindByEmailAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            store.SetNormalizedEmailAsync(user1, lookup.Normalize(user1.Email));

            // act
            store.CreateAsync(user1);

            var r = store.FindByEmailAsync(user1.NormalizedEmail);

            // assert
            Assert.AreEqual(user1.Id, r.Result.Id);
        }

        [TestMethod]
        public void CanFindByIdAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.CreateAsync(user1);

            var r = store.FindByIdAsync(user1.Id);

            // assert
            Assert.AreEqual(user1.Email, r.Result.Email);
        }

        [TestMethod]
        public void CanFindByLoginAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var login1 = new UserLoginInfo("LoginProvider1", "ProviderKey1", "DisplayNam1");
            
            store.AddLoginAsync(user1, login1);

            store.CreateAsync(user1);

            // act
            var r = store.FindByLoginAsync("LoginProvider1", "ProviderKey1");

            // assert
            Assert.AreEqual(user1.UserName, r.Result.UserName);
            Assert.AreEqual(user1.Email, r.Result.Email);
        }

        [TestMethod]
        public void CanFindByNameAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            store.SetNormalizedUserNameAsync(user1, lookup.Normalize(user1.UserName));

            store.CreateAsync(user1);
            // act
            var r = store.FindByNameAsync(user1.NormalizedUserName);

            // assert
            Assert.AreEqual(user1.UserName, r.Result.UserName);
            Assert.AreEqual(user1.Email, r.Result.Email);
        }

        [TestMethod]
        public void CanGetAccessFailedCountAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true,
                AccessFailedCount = 10
            };

            // act
            var r = store.GetAccessFailedCountAsync(user1);

            // assert
            Assert.AreEqual(10, r.Result);
        }

        [TestMethod]
        public void CanSetGetEmailAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                EmailConfirmed = true
            };

            // act
            store.SetEmailAsync(user1, "user1@user.com");
            var r = store.GetEmailAsync(user1);

            // assert
            Assert.AreEqual("user1@user.com", r.Result);
        }

        [TestMethod]
        public void CanSetGetEmailConfirmedAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com"
            };

            // act
            store.SetEmailConfirmedAsync(user1, true);
            var r = store.GetEmailConfirmedAsync(user1);

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanGetLockoutEnabledAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetLockoutEnabledAsync(user1, true);
            var r = store.GetLockoutEnabledAsync(user1);

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanSetGetLockoutEndDateAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetLockoutEnabledAsync(user1, true);
            store.SetLockoutEndDateAsync(user1, Convert.ToDateTime("4/2/2007"));

            var r = store.GetLockoutEndDateAsync(user1);

            // assert
            Assert.AreEqual("4/2/2007 12:00:00 AM +08:00", r.Result.ToString());
        }

        [TestMethod]
        public void CanSetGetNormalizedEmailAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetNormalizedEmailAsync(user1, lookup.Normalize(user1.Email));
            var r = store.GetNormalizedEmailAsync(user1);

            // assert
            Assert.AreEqual(lookup.Normalize(user1.Email), r.Result);
        }

        [TestMethod]
        public void CanSetGetNormalizedUserNameAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetNormalizedUserNameAsync(user1, lookup.Normalize(user1.UserName));
            var r = store.GetNormalizedUserNameAsync(user1);

            // assert
            Assert.AreEqual(lookup.Normalize(user1.UserName), r.Result);
        }

        [TestMethod]
        public void CanSetGetPasswordHashAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetPasswordHashAsync(user1, "this.is.password.hash");
            var r = store.GetPasswordHashAsync(user1);

            // assert
            Assert.AreEqual("this.is.password.hash", r.Result);
        }

        [TestMethod]
        public void CanSetGetPhoneNumberAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetPhoneNumberAsync(user1, "111 222 469");
            var r = store.GetPhoneNumberAsync(user1);

            // assert
            Assert.AreEqual("111 222 469", r.Result);
        }

        [TestMethod]
        public void CanSetGetPhoneNumberConfirmedAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetPhoneNumberConfirmedAsync(user1, true);
            var r = store.GetPhoneNumberConfirmedAsync(user1);

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanGetSecurityStampAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var r = store.GetSecurityStampAsync(user1);

            // assert
            Assert.AreEqual(user1.SecurityStamp, r.Result);
        }

        [TestMethod]
        public void CanSetGetTwoFactorEnabledAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetTwoFactorEnabledAsync(user1, true);
            var r = store.GetTwoFactorEnabledAsync(user1);

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanGetUserIdAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var r = store.GetUserIdAsync(user1);

            // assert
            Assert.AreEqual(user1.Id, r.Result);
        }

        [TestMethod]
        public void CanSetGetUserNameAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetUserNameAsync(user1, "User2");
            var r = store.GetUserNameAsync(user1);

            // assert
            Assert.AreEqual(user1.UserName, r.Result);
        }

        [TestMethod]
        public void CanGetUsersForClaimAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var list = new List<Claim>();
            var claim1 = new Claim("ClaimType1", "ClaimValue1", "ClaimValueType1");

            list.Add(claim1);

            // act
            store.AddClaimsAsync(user1, list);
            store.CreateAsync(user1);

            var r = store.GetUsersForClaimAsync(claim1);

            // assert
            Assert.AreEqual(1, r.Result.Count());
        }

        [TestMethod]
        public void CanGetUsersInRoleAsync()
        {
            var roleStore = new RoleStore<IdentityRole>(Session);
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            roleStore.CreateAsync(role1);
            store.AddToRoleAsync(user1, "Role1");
            store.CreateAsync(user1);

            var r = store.GetUsersInRoleAsync("Role1");

            // assert
            Assert.AreEqual(1, r.Result.Count());
        }

        [TestMethod]
        public void CanHasPasswordAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            store.SetPasswordHashAsync(user1, "this.is.password.hash");
            var r = store.HasPasswordAsync(user1);

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanIncrementAccessFailedCountAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true,
                AccessFailedCount = 10
            };

            // act
            var r = store.IncrementAccessFailedCountAsync(user1);

            // assert
            Assert.AreEqual(11, r.Result);
        }

        [TestMethod]
        public void CanIsInRoleAsync()
        {
            var roleStore = new RoleStore<IdentityRole>(Session);
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            roleStore.CreateAsync(role1);
            store.AddToRoleAsync(user1, "Role1");
            store.CreateAsync(user1);

            var r = store.IsInRoleAsync(user1, "Role1");

            // assert
            Assert.AreEqual(true, r.Result);
        }

        [TestMethod]
        public void CanRemoveClaimsAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var list = new List<Claim>();
            var claim1 = new Claim("ClaimType1", "ClaimValue1", "ClaimValueType1");

            list.Add(claim1);

            // act
            store.AddClaimsAsync(user1, list);
            store.RemoveClaimsAsync(user1, list);

            var r = store.GetClaimsAsync(user1);

            // assert
            Assert.AreEqual(0, r.Result.Count());
        }

        [TestMethod]
        public void CanRemoveLoginAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var login1 = new UserLoginInfo("LoginProvider1", "ProviderKey1", "DisplayNam1");

            // act
            store.AddLoginAsync(user1, login1);
            store.RemoveLoginAsync(user1, login1.LoginProvider, login1.ProviderKey);

            var r = store.GetLoginsAsync(user1);

            // assert
            Assert.AreEqual(0, r.Result.Count());
        }

        [TestMethod]
        public void CanRemoveFromRoleAsync()
        {
            var roleStore = new RoleStore<IdentityRole>(Session);
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            roleStore.CreateAsync(role1);

            store.AddToRoleAsync(user1, "Role1");
            store.RemoveFromRoleAsync(user1, "Role1");

            var r = store.GetRolesAsync(user1);

            // assert
            Assert.AreEqual(0, r.Result.Count());
        }

        [TestMethod]
        public void CanReplaceClaimAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var list1 = new List<Claim>();
            var claim1 = new Claim("ClaimType1", "ClaimValue1", "ClaimValueType1");

            list1.Add(claim1);
            
            var claim2 = new Claim("ClaimType2", "ClaimValue2", "ClaimValueType2");
            
            // act
            store.AddClaimsAsync(user1, list1);
            store.ReplaceClaimAsync(user1, claim1, claim2);

            var r = store.GetClaimsAsync(user1);

            // assert
            Assert.AreEqual(1, r.Result.Count());
            Assert.AreEqual("ClaimType2", r.Result[0].Type);
            Assert.AreEqual("ClaimValue2", r.Result[0].Value);
        }

        [TestMethod]
        public void CanResetAccessFailedCountAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true,
                AccessFailedCount = 10
            };

            // act
            store.ResetAccessFailedCountAsync(user1);

            var r = store.GetAccessFailedCountAsync(user1);

            // assert
            Assert.AreEqual(0, r.Result);
        }

        [TestMethod]
        public void CanSetGetTokenAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var token1 = new IdentityUserToken()
            {
                UserId = user1.Id,
                LoginProvider = "LoginProvider1",
                Name = "Name1",
                Value = "Value1"
            };

            // act
            store.SetTokenAsync(user1, token1.LoginProvider, token1.Name, token1.Value);
            store.CreateAsync(user1);

            var r = store.GetTokenAsync(user1, token1.LoginProvider, token1.Name);

            // assert
            Assert.AreEqual(token1.Value, r.Result);
        }

        [TestMethod]
        public void CanRemoveTokenAsync()
        {
            var store = new UserStore<IdentityUser>(Session);

            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var token1 = new IdentityUserToken()
            {
                UserId = user1.Id,
                LoginProvider = "LoginProvider1",
                Name = "Name1",
                Value = "Value1"
            };

            // act
            store.SetTokenAsync(user1, token1.LoginProvider, token1.Name, token1.Value);
            store.CreateAsync(user1);

            store.RemoveTokenAsync(user1, token1.LoginProvider, token1.Name);
            var r = store.GetTokenAsync(user1, token1.LoginProvider, token1.Name);

            // assert
            Assert.AreEqual(string.Empty, r.Result);
        }
    }
}
