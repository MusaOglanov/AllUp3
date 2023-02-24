using AllUp3.DAL;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db)
        {
            _db = db; 
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            List<Product> products = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductDetail)
                .Include(p => p.Brand)
                .Include(p => p.ProductTags)
                .ThenInclude(p => p.Tag)
                .Include(p => p.ProductCategories)
                .ThenInclude(p => p.Category)
                .ToListAsync();
            return View(products);
        }
        #endregion

        #region Create

        #region get
        public async Task<IActionResult> Create()
        {
            ViewBag.Brands=await _db.Brands.ToListAsync();
            ViewBag.Tags=await _db.Tags.ToListAsync();
            ViewBag.MainCategories = await _db.Categories.Where(c => c.IsMain).ToListAsync();
            Category firstMainCategory=await _db.Categories.Include(x=>x.Children).FirstOrDefaultAsync(c=>c.IsMain);
            ViewBag.ChildCategories = firstMainCategory.Children;
            return View();
        }
        #endregion

        #endregion
    }
}
