using AllUp3.DAL;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp3.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class TagsController : Controller
    {
        private readonly AppDbContext _db;
        public TagsController(AppDbContext db)
        {
            _db = db;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<Tag> tags = await _db.Tags.ToListAsync();
            return View(tags);
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
        public async Task<IActionResult> Create(Tag tag)
        {
            bool IsExist = await _db.Tags.AnyAsync(t=>t.Name==tag.Name);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This name already is exist");
                return View();
            }


            await _db.Tags.AddAsync(tag);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #endregion
    }
}
