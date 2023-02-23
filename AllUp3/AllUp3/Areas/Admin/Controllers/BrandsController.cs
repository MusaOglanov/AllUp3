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

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Brand> brands = await _db.Brands.ToListAsync();
            return View(brands);
        }
        #endregion

        #region Create

        #region get
        public IActionResult Create()
        {
            return View();
        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand brand)
        {
            bool IsExist = await _db.Brands.AnyAsync(t => t.Name == brand.Name);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This name already is exist");
                return View();
            }


            await _db.Brands.AddAsync(brand);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Brand brand = await _db.Brands.FirstOrDefaultAsync(t => t.Id == id);
            if (brand == null)
            {
                return BadRequest();
            }
            return View(brand);
        }


        #endregion

        #region Update

        #region get
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(t => t.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }
            return View(dbBrand);

        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Brand brand, int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(t => t.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }
            bool IsExist = await _db.Brands.AnyAsync(t => t.Name == brand.Name && t.Id != id);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This tag already is exist ");
            }

            dbBrand.Name = brand.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion

        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Brand dbBrand = await _db.Brands.FirstOrDefaultAsync(t => t.Id == id);
            if (dbBrand == null)
            {
                return BadRequest();
            }

            if (dbBrand.IsDeactive)
            {
                dbBrand.IsDeactive = false;
            }
            else
            {
                dbBrand.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
