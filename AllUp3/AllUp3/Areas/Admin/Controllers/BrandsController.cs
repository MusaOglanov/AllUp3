using AllUp3.DAL;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly AppDbContext _db;
        public BrandsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult > Index()
        {
            List<Brand> brands = await _db.Brands.ToListAsync();
            return View(brands);
        }
    }
}
