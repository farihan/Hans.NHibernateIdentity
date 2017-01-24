using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hans.AspNetCore.Identity.NHibernate.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net.Http;
using Microsoft.Extensions.PlatformAbstractions;

namespace Hans.AspNetCore.Identity.NHibernate.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void TestIndex()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void TestAbout()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.About();

            // Assert
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Your application description page1.", viewResult.ViewData["Message"]);
        }

        [TestMethod]
        public void TestContact()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Contact();

            // Assert
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
            Assert.AreEqual("Your contact page.", viewResult.ViewData["Message"]);
        }

        [TestMethod]
        public void TestError()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Error();

            // Assert
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void TestRedirectToLocal()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.RedirectToLocal("");

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
