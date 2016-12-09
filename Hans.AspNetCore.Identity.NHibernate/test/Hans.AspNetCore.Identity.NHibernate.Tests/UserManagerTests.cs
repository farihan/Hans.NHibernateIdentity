using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    public class UserManagerTests : BaseTests
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanCreateUserManager()
        {
            // arrange
            // act
            var manager = GetUserManager(Session);

            // assert
            Assert.IsNotNull(manager);
        }
    }
}
