using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AllUp3.Controllers
{
    public class HomeController : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}