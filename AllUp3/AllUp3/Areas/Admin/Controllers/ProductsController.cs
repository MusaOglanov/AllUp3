using AllUp3.DAL;
using AllUp3.Helpers;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AllUp3.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;
        public ProductsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;

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
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.MainCategories = await _db.Categories.Where(c => c.IsMain).ToListAsync();
            Category firstMainCategory = await _db.Categories.Include(x => x.Children).FirstOrDefaultAsync(c => c.IsMain);
            ViewBag.ChildCategories = firstMainCategory.Children;
            return View();
        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, int brandId, int[] tagsId, int mainCatId, int? childCatId)
        {
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.MainCategories = await _db.Categories.Where(c => c.IsMain).ToListAsync();
            Category firstMainCategory = await _db.Categories.Include(x => x.Children).FirstOrDefaultAsync(c => c.IsMain);
            ViewBag.ChildCategories = firstMainCategory.Children;

            List<ProductImage> productImages = new List<ProductImage>();
            if (product.Photos == null)
            {
                ModelState.AddModelError("Photo", "Please slect image ");
                return View();
            }
            foreach (IFormFile photo in product.Photos)
            {
                
                if (!photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Please slect image file");
                    return View();
                }
                if (photo.IsOlder2MB())
                {
                    ModelState.AddModelError("Photo", "Max 2MB");
                    return View();
                }
                string folder = Path.Combine(_env.WebRootPath, "assets", "images","product");
                ProductImage productImage = new ProductImage
                {
                    Image = await photo.SaveImageAsync(folder),
                };

                productImages.Add(productImage);
            }


            product.ProductImages = productImages;
            product.BrandId = brandId;

            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in tagsId)
            {
                ProductTag productTag = new ProductTag
                {
                    TagId=tagId,

                };
                productTags.Add(productTag);
            }
            product.ProductTags = productTags;

            List<ProductCategory> productCategories = new List<ProductCategory>();

            ProductCategory mainProductCategory = new ProductCategory
            {
                CategoryId = mainCatId,
            };
            productCategories.Add(mainProductCategory);

            if(childCatId != null)
            {

                ProductCategory childProductCategory = new ProductCategory
                {
                    CategoryId = (int) childCatId,
                };
                productCategories.Add(childProductCategory);


            }

            product.ProductCategories = productCategories;

            await _db.Products.AddAsync(product);
            await _db.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
        #endregion

        #endregion

        [ActionName("LoadChild")]
        [HttpPost]
        public async Task<IActionResult> LoadChildCategories(int mainId)
        {
            List<Category> categories = await _db.Categories.Where(x => x.ParentId == mainId).ToListAsync();
            return PartialView("_LoadChildCategories", categories);
        }
    }
}
