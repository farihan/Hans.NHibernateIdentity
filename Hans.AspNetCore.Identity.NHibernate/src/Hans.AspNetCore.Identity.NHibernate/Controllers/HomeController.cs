using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Hans.AspNetCore.Identity.NHibernate.Data.Domains;

namespace Hans.AspNetCore.Identity.NHibernate.Controllers
{
    public class HomeController : Controller
    {
        //private readonly UserManager<IdentityUser> userManager;
        //private readonly SignInManager<IdentityUser> signInManager;

        //public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        //{
        //    this.userManager = userManager;
        //    this.signInManager = signInManager;
        //}

        public IActionResult Index()
        {
            //// arrange
            //var user1 = new IdentityUser("User6")
            //{
            //    Email = "user6@user.com",
            //    EmailConfirmed = true
            //};

            //var role1 = new IdentityRole("Role1");

            //// act
            //userManager.CreateAsync(user1, "password");

            //var result = signInManager.SignInAsync(user1, false);

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult RedirectToLocal(string returnUrl)
        {
            if(string.IsNullOrWhiteSpace(returnUrl))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
