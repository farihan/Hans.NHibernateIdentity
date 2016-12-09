using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Hans.AspNetCore.Identity.NHibernate.Data.Persistence;
using System.Linq;
using Hans.AspNetCore.Identity.NHibernate.Data;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    [TestClass]
    public class RoleStoreTests : BaseTests
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanRoles()
        {
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");
            var role2 = new IdentityRole("Role2");
            var role3 = new IdentityRole("Role3");
            var role4 = new IdentityRole("Role4");
            var role5 = new IdentityRole("Role5");

            // act
            store.CreateAsync(role1);
            store.CreateAsync(role2);
            store.CreateAsync(role3);
            store.CreateAsync(role4);
            store.CreateAsync(role5);

            // assert
            Assert.AreEqual(5, store.Roles.Count());
        }

        [TestMethod]
        public void CanCreateUpdateDeleteRole()
        {
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");

            // act
            store.CreateAsync(role1);
            var r1 = store.Roles.FirstOrDefault();

            // assert
            Assert.AreEqual(role1.Id, r1.Id);

            r1.Name = "Role2";
            store.UpdateAsync(r1);
            var r2 = store.Roles.FirstOrDefault();

            // assert
            Assert.AreEqual(r1.Name, r2.Name);

            store.DeleteAsync(r1);
            var r3 = store.Roles.FirstOrDefault();

            // assert
            Assert.IsNull(r3);
        }

        [TestMethod]
        public void CanFindByIdAsync()
        {
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");

            // act
            store.CreateAsync(role1);

            var r = store.FindByIdAsync(role1.Id);

            // assert
            Assert.AreEqual("Role1", r.Result.Name);
        }

        [TestMethod]
        public void CanFindByNameAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");
            role1.NormalizedName = lookup.Normalize(role1.Name);

            // act
            store.CreateAsync(role1);

            var r = store.FindByNameAsync(role1.NormalizedName);

            // assert
            Assert.AreEqual(role1.Id, r.Result.Id);
        }

        [TestMethod]
        public void CanSetGetNormalizedRoleNameAsync()
        {
            var lookup = new LowerInvariantLookupNormalizer();
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");

            // act
            store.SetNormalizedRoleNameAsync(role1, lookup.Normalize(role1.Name));

            var r = store.GetNormalizedRoleNameAsync(role1);

            // assert
            Assert.AreEqual(lookup.Normalize(role1.Name), r.Result);
        }

        [TestMethod]
        public void CanGetRoleIdAsync()
        {
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole("Role1");

            // act

            var r = store.GetRoleIdAsync(role1);

            // assert
            Assert.AreEqual(role1.Id, r.Result);
        }

        [TestMethod]
        public void CanSetGetRoleNameAsync()
        {
            var store = new RoleStore<IdentityRole>(Session);

            // arrange
            var role1 = new IdentityRole();

            // act
            store.SetRoleNameAsync(role1, "Role1000");
            var r = store.GetRoleNameAsync(role1);

            // assert
            Assert.AreEqual("Role1000", r.Result);
        }
    }
}
