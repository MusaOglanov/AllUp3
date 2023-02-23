using AllUp3.DAL;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Tag tag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (tag == null)
            {
                return BadRequest();
            }
            return View(tag);
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
            Tag dbTag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTag == null)
            {
                return BadRequest();
            }
            return View(dbTag);
        
        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Tag tag,int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Tag dbTag = await _db.Tags.FirstOrDefaultAsync(t => t.Id == id);
            if (dbTag == null)
            {
                return BadRequest();
            }
            bool IsExist= await _db.Tags.AnyAsync(t=>t.Name==tag.Name&&t.Id!=id);
            if (IsExist)
            {
                ModelState.AddModelError("Name", "This tag already is exist ");
            }

            dbTag.Name = tag.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
        #endregion

        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if(id==null)
            {
                return NotFound();  
            }   
            Tag dbTag=await _db.Tags.FirstOrDefaultAsync(t=>t.Id==id);
            if (dbTag == null)
            {
                return BadRequest();
            }

            if (dbTag.IsDeactive )
            {
                dbTag.IsDeactive = false;
            }
            else
            {
                dbTag.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
