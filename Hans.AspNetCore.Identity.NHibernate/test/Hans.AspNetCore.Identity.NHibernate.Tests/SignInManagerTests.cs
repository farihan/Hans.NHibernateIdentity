using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    [TestClass]
    public class SignInManagerTests : BaseTests
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanGetSignInManager()
        {
            // arrange
            // act
            var manager = GetSignInManager(Session);

            // assert
            Assert.IsNotNull(manager);
        }

        // for future reference
        // ====================
        //[TestMethod]
        //public void CanPasswordSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.PasswordSignInAsync(user1, PASSWORD, false, false);

        //    // assert
        //    // because no app.UseIdentity()
        //    Assert.IsTrue(result.IsFaulted);
        //}

        //[TestMethod]
        //public void CanSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.SignInAsync(user1, false);

        //    // assert
        //    // because no app.UseIdentity()
        //    Assert.IsTrue(result.IsFaulted);
        //}

        //[TestMethod]
        //public void CanSignOutAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.SignOutAsync();

        //    // assert
        //    // because no app.UseIdentity()
        //    Assert.IsTrue(result.IsFaulted);
        //}

        //[TestMethod]
        //public void CanCanSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.CanSignInAsync(user1);

        //    // assert
        //    Assert.IsTrue(result.Result);
        //}

        //[TestMethod]
        //public void CanCheckPasswordSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.CheckPasswordSignInAsync(user1, PASSWORD, false);

        //    // assert
        //    Assert.AreEqual(SignInResult.Success, result.Result);
        //}

        //[TestMethod]
        //public void CanCreateUserPrincipalAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.CreateUserPrincipalAsync(user1);

        //    // assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void CanConfigureExternalAuthenticationProperties()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.ConfigureExternalAuthenticationProperties("provider", "url", user1.Id);

        //    // assert
        //    Assert.IsNotNull(result);
        //    Assert.AreEqual("url", result.Items[".redirect"]);
        //    Assert.AreEqual("provider", result.Items["LoginProvider"]);
        //}

        //[TestMethod]
        //public void CanGetExternalLoginInfoAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.GetExternalLoginInfoAsync();

        //    // assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void CanExternalLoginSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    var provider1 = "LoginProvider1";
        //    var key1 = "ProviderKey1";
        //    var name1 = "DisplayName1";
        //    var login1 = new UserLoginInfo(provider1, key1, name1);

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);
        //    userManager.AddLoginAsync(user1, login1);

        //    var result = signInManager.ExternalLoginSignInAsync(login1.LoginProvider, login1.ProviderKey, false);

        //    // assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void CanGetTwoFactorAuthenticationUserAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.GetTwoFactorAuthenticationUserAsync();

        //    // assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void CanTwoFactorSignInAsync()
        //{
        //    // arrange
        //    var user1 = new IdentityUser("User1")
        //    {
        //        Email = "user1@user.com",
        //        EmailConfirmed = true
        //    };

        //    // act
        //    var userManager = GetUserManager(Session);
        //    var signInManager = GetSignInManager(Session);

        //    userManager.CreateAsync(user1, PASSWORD);

        //    var result = signInManager.TwoFactorSignInAsync("provider", "code", false, false);

        //    // assert
        //    Assert.IsNotNull(result);
        //}
    }
}
