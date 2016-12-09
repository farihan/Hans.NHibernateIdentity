using Hans.AspNetCore.Identity.NHibernate.Data.Domains;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    [TestClass]
    public class RoleManagerTests : BaseTests
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanCreateRoleManager()
        {
            // arrange
            // act
            var manager = GetRoleManager(Session);

            // assert
            Assert.IsNotNull(manager);
        }

        [TestMethod]
        public void CanCreateRole()
        {
            // arrange
            var roleName = "Admin";
            var role = new IdentityRole(roleName);

            var manager = GetRoleManager(Session);
            manager.CreateAsync(role);

            // act
            var foundRole = manager.GetRoleNameAsync(role);
            //var foundRole = manager.FindByNameAsync(roleName);

            // assert
            Assert.AreEqual(roleName, foundRole.Result);
            //Assert.AreEqual(role.Id, foundRole.Result.Id);
        }
    }
}
