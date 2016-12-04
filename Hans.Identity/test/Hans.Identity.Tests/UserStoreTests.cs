using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.Identity.Data.Domains;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNet.Identity;
using Hans.Identity.Data.Persistence;

namespace Hans.Identity.Tests
{
    [TestClass]
    public class UserStoreTests : BaseTest
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanAddClaimAndGetClaims()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var type1 = "Type1";
            var value1 = "Value1";
            var claim1 = new Claim(type1, value1);

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.AddClaim(user1.Id, claim1);

            var claims = manager.GetClaims(user1.Id);

            // assert
            Assert.AreEqual(1, claims.Count);
            Assert.AreEqual(type1, claims[0].Type);
            Assert.AreEqual(value1, claims[0].Value);
        }

        [TestMethod]
        public void CanAddClaimAndRemoveClaim()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var type1 = "Type1";
            var value1 = "Value1";
            var claim1 = new Claim(type1, value1);

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.AddClaim(user1.Id, claim1);
            manager.RemoveClaim(user1.Id, claim1);

            var claims = manager.GetClaims(user1.Id);

            // assert
            Assert.AreEqual(0, claims.Count);
        }

        [TestMethod]
        public void CanAddLoginAndGetLogin()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var provider1 = "LoginProvider1";
            var key1 = "ProviderKey1";
            var login1 = new UserLoginInfo(provider1, key1);

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.AddLoginAsync(user1.Id, login1);

            var login = manager.GetLogins(user1.Id);

            // assert
            Assert.AreEqual(1, login.Count);
        }

        [TestMethod]
        public void CanAddLoginAndRemoveLogin()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var provider1 = "LoginProvider1";
            var key1 = "ProviderKey1";
            var login1 = new UserLoginInfo(provider1, key1);

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.AddLogin(user1.Id, login1);
            manager.RemoveLogin(user1.Id, login1);

            var logins = manager.GetLogins(user1.Id);

            // assert
            Assert.AreEqual(0, logins.Count);
        }

        [TestMethod]
        public void CanAddRoleAndGetRole()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            var roleManager = GetRoleManager(Session);
            var userManager = GetUserManager(Session);

            roleManager.Create(role1);
            userManager.Create(user1, "password");
            
            userManager.AddToRole(user1.Id, role1.Name);

            var roles = userManager.GetRoles(user1.Id);

            // assert
            Assert.AreEqual(1, roles.Count);
        }

        [TestMethod]
        public void CanAddRoleAndRemoveRole()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            var roleManager = GetRoleManager(Session);
            var userManager = GetUserManager(Session);

            roleManager.Create(role1);
            userManager.Create(user1, "password");

            userManager.AddToRole(user1.Id, role1.Name);

            userManager.RemoveFromRoles(user1.Id, role1.Name);

            var roles = userManager.GetRoles(user1.Id);

            // assert
            Assert.AreEqual(0, roles.Count);
        }

        [TestMethod]
        public void CanCreateUser()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var userManager = GetUserManager(Session);
            userManager.Create(user1, "password");

            var user = userManager.FindByName(user1.UserName);

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CanUpdateUser()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var userManager = GetUserManager(Session);
            userManager.Create(user1, "password");

            var updateUser = userManager.FindByName("User1");
            updateUser.Email = "user1update@user.com";
            updateUser.AccessFailedCount = 1;

            userManager.Update(updateUser);

            var user = userManager.FindByName("User1");

            // assert
            Assert.IsNotNull(user);
            Assert.AreEqual(1, user.AccessFailedCount);
            Assert.AreEqual("user1update@user.com", user.Email);
        }

        [TestMethod]
        public void CanDeleteUser()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var userManager = GetUserManager(Session);
            userManager.Create(user1, "password");
            var deleteUser = userManager.FindByName(user1.UserName);
            userManager.Delete(deleteUser);
            var user = userManager.FindByName(user1.UserName);

            // assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void CanFindUserByLogin()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var provider1 = "LoginProvider1";
            var key1 = "ProviderKey1";
            var login1 = new UserLoginInfo(provider1, key1);

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.AddLogin(user1.Id, login1);

            var user = manager.Find(login1);

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CanFindUserByEmail()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            var user = manager.FindByEmail("user1@user.com");

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CanFindUserById()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            var user = manager.FindById(user1.Id);

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CanFindUserByName()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            var user = manager.FindByName("User1");

            // assert
            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void CanSetAndGetAccessFailedCount()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            
            user1.AccessFailedCount = 1;

            var count = manager.GetAccessFailedCount(user1.Id);

            // assert
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void CanSetAndGetEmail()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.SetEmail(user1.Id, "user1updated@user.com");
            var email = manager.GetEmail(user1.Id);

            // assert
            Assert.AreEqual("user1updated@user.com", email);
        }

        [TestMethod]
        public void CanGetAndSetEmailConfirmed()
        {
            throw new NotImplementedException("UserManager does not have GetAndSetEmailConfirmed method.");
        }

        [TestMethod]
        public void CanSetAndGetLockoutEnabled()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.SetLockoutEnabled(user1.Id, true);
            var lockout = manager.GetLockoutEnabled(user1.Id);

            // assert
            Assert.AreEqual(true, lockout);
        }

        [TestMethod]
        public void CanSetAndGetLockoutEndDate()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");
            manager.SetLockoutEnabled(user1.Id, true);

            manager.SetLockoutEndDate(user1.Id, Convert.ToDateTime("4/2/2007"));
            var enddate = manager.GetLockoutEndDate(user1.Id);

            // assert
            Assert.AreEqual("4/2/2007 12:00:00 AM +08:00", enddate.ToString());
        }

        [TestMethod]
        public void CanGetAndSetPasswordHash()
        {
            throw new NotImplementedException("UserManager does not have GetAndSetPasswordHash method.");
        }

        [TestMethod]
        public void CanSetAndGetPhoneNumber()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.SetPhoneNumber(user1.Id, "111-222-333");
            var phone = manager.GetPhoneNumber(user1.Id);

            // assert
            Assert.AreEqual("111-222-333", phone);
        }

        [TestMethod]
        public void CanSetAndGetPhoneNumberConfirmed()
        {
            throw new NotImplementedException("UserManager does not have SetAndGetPhoneNumberConfirmed method.");
        }

        [TestMethod]
        public void CanSetAndGetSecurityStamp()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.UpdateSecurityStamp(user1.Id);
            var securityStamp = manager.GetSecurityStamp(user1.Id);

            // assert
            Assert.IsNotNull(securityStamp);
        }

        [TestMethod]
        public void CanSetAndGetTwoFactorEnabled()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.SetTwoFactorEnabled(user1.Id, true);
            var enable = manager.GetTwoFactorEnabled(user1.Id);

            // assert
            Assert.AreEqual(true, enable);
        }

        [TestMethod]
        public void CanHasPassword()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            var result = manager.HasPassword(user1.Id);

            // assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void CanIncrementAccessFailedCount()
        {
            throw new NotImplementedException("UserManager does not have IncrementAccessFailedCount method.");
        }

        [TestMethod]
        public void CanIsInRole()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            var role1 = new IdentityRole("Role1");

            // act
            var roleManager = GetRoleManager(Session);
            var userManager = GetUserManager(Session);

            roleManager.Create(role1);
            userManager.Create(user1, "password");

            userManager.AddToRole(user1.Id, role1.Name);

            var inrole = userManager.IsInRole(user1.Id, "Role1");

            // assert
            Assert.AreEqual(true, inrole);
        }

        [TestMethod]
        public void CanResetAccessFailedCount()
        {
            // arrange
            var user1 = new IdentityUser("User1")
            {
                Email = "user1@user.com",
                EmailConfirmed = true
            };

            // act
            var manager = GetUserManager(Session);
            manager.Create(user1, "password");

            manager.ResetAccessFailedCount(user1.Id);

            var user = manager.FindByName("User1");
            // assert
            Assert.AreEqual(0, user.AccessFailedCount);
        }
    }
}
