using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Hans.Identity.Controllers;
using Hans.Identity.Data;
using Hans.Identity.Data.Domains;
using Hans.Identity.Data.Persistence;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hans.Identity.Tests
{
    [TestClass]
    public class BasicTests : BaseTest
    {
        [TestInitialize]
        public void BeforeEachTest()
        {
            CreateDatabase(Connection);
        }

        [TestMethod]
        public void CanInitializePersistenceConfiguration()
        {
            //arrange
            var config = new PersistenceConfiguration();

            //act
            var sf = config.Initialize(Connection);

            //assert
            Assert.IsNotNull(sf);
        }

        [TestMethod]
        public void CanInstantiateRepository()
        {
            //arrange
            var config = new PersistenceConfiguration();
            var sf = config.Initialize(Connection);

            //act
            var repository = new Repository<IdentityRole>(sf.OpenSession());

            //assert
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void CanInstantiateController()
        {
            //arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            //assert
            Assert.AreEqual("Microsoft.AspNetCore.Mvc.ViewResult", result.GetType().ToString());
        }

        [TestMethod]
        public void CanInstantiateRoleManager()
        {
            //arrange
            var config = new PersistenceConfiguration();
            var sf = config.Initialize(Connection);

            //act
            var manager = GetRoleManager(sf.OpenSession());

            //assert
            Assert.IsNotNull(manager);
        }

        [TestMethod]
        public void CanInstantiateUserManager()
        {
            //arrange
            var config = new PersistenceConfiguration();
            var sf = config.Initialize(Connection);

            //act
            var manager = GetUserManager(sf.OpenSession());

            //assert
            Assert.IsNotNull(manager);
        }
    }
}
