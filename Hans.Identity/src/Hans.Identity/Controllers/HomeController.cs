using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Hans.Identity.Data.Persistence;
using Hans.Identity.Data.Domains;

namespace Hans.Identity.Controllers
{
    public class HomeController : Controller
    {
        //private IRepository<AspNetUser> userRepository;

        //public HomeController(IRepository<AspNetUser> userRepository)
        //{
        //    this.userRepository = userRepository;
        //}

        public IActionResult Index()
        {
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
    }
}
