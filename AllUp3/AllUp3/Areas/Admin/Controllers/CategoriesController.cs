using AllUp3.DAL;
using AllUp3.Helpers;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace AllUp3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public CategoriesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories
                .Include(x => x.Children)
                .Include(x => x.Parent)
                .ToListAsync();
            return View(categories);
        }
        #endregion

        #region Create

        #region get
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCatgeories = await _db.Categories.Where(x => x.IsMain).ToListAsync();
            return View();
        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category,int mainCatId)
        {
            ViewBag.MainCatgeories = await _db.Categories.Where(x => x.IsMain).ToListAsync();
            if (category.IsMain)
            {
                bool isExist = await _db.Categories.AnyAsync(x=>x.Name==category.Name);
                if(isExist)
                {
                    ModelState.AddModelError("Name", "This Category Already i exist!!!");
                    return View();
                }
                if (category.Photo == null)
                {
                    ModelState.AddModelError("Photo", "Please slect image ");
                    return View();
                }
                if (!category.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please slect image file");
                    return View();
                }
                if (category.Photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Max 2MB");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets","images");
                category.Image = await category.Photo.SaveImageAsync(folder);
            }
            else
            {
                category.ParentId = mainCatId;
            }

            await _db.Categories.AddAsync(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion
    }
}
