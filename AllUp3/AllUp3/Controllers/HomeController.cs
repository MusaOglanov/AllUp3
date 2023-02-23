using AllUp3.DAL;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AllUp3.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Category> mainCategories = await _db.Categories
                .Where(x => x.IsMain).ToListAsync();
            return View(mainCategories);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}