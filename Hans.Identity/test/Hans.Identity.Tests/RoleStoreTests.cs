using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.Identity.Data.Domains;
using Microsoft.AspNet.Identity;
using Hans.Identity.Data;
using Hans.Identity.Data.Persistence;
using System.Linq;

namespace Hans.Identity.Tests
{
    [TestClass]
    public class RoleStoreTests : BaseTest
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanCreateRole()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.Create(role);

            // act
            var repository = new Repository<IdentityRole>(Session);
            var foundRole = repository.FindAll().FirstOrDefault();

            // assert
            Assert.AreEqual(roleName, foundRole.Name);
            Assert.AreEqual(role.Id, foundRole.Id);
        }

        [TestMethod]
        public void CanDeleteRole()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.Create(role);

            var repository = new Repository<IdentityRole>(Session);
            var foundRole = repository.FindAll().FirstOrDefault();

            // act
            manager.Delete(foundRole);
            
            var deletedRole = repository.FindAll().FirstOrDefault();

            // assert
            Assert.IsNull(deletedRole);
        }

        [TestMethod]
        public void CanUpdateRole()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.Create(role);

            var repository = new Repository<IdentityRole>(Session);
            var foundRole = repository.FindAll().FirstOrDefault();
            foundRole.Name = "SuperUser";

            // act
            manager.Update(foundRole);

            var updatedRole = repository.FindAll().FirstOrDefault();

            // assert
            Assert.AreEqual(foundRole.Name, updatedRole.Name);
            Assert.AreEqual(foundRole.Id, updatedRole.Id);
        }

        [TestMethod]
        public void CanFindById()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.Create(role);

            // act
            var foundRole = manager.FindByName(role.Name);

            // assert
            Assert.AreEqual(role.Name, foundRole.Name);
            Assert.AreEqual(role.Id, foundRole.Id);
        }

        [TestMethod]
        public void CanFindByName()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.Create(role);

            // act
            var foundRole = manager.FindById(role.Id);

            // assert
            Assert.AreEqual(role.Name, foundRole.Name);
            Assert.AreEqual(role.Id, foundRole.Id);
        }

        [TestMethod]
        public void CanGetRoles()
        {
            // arrange
            var roleName1 = "Admin 1";
            var role1 = new IdentityRole(roleName1);

            var roleName2 = "Admin 2";
            var role2 = new IdentityRole(roleName2);

            var roleName3 = "Admin 3";
            var role3 = new IdentityRole(roleName3);

            var manager = GetRoleManager(Session);
            manager.Create(role1);
            manager.Create(role2);
            manager.Create(role3);

            // act
            var roles = manager.Roles;

            // assert
            Assert.AreEqual(3, roles.Count());
        }
    }
}
