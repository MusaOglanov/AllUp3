using AllUp3.DAL;
using AllUp3.Helpers;
using AllUp3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

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

        #region Update

        #region get
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null)
            {
                return NotFound();


            }
            Product? dbProduct = await _db.Products
                .Include(x => x.ProductDetail)
                .Include(x => x.ProductImages)
                .Include(x=>x.ProductTags)
                .Include(x=>x.ProductCategories)
                .ThenInclude(x=>x.Category)
                .ThenInclude(x => x.Children)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.MainCategories = await _db.Categories.Where(c => c.IsMain).ToListAsync();
            ViewBag.ChildCategories = dbProduct.ProductCategories.FirstOrDefault().Category.Children;
            return View(dbProduct);

        }
        #endregion

        #region post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Product product, int brandId, int[] tagsId, int mainCatId, int? childCatId)
        {
            if (id == null)
            {
                return NotFound();


            }
            Product? dbProduct = await _db.Products
                           .Include(x => x.ProductDetail)
                           .Include(x => x.ProductImages)
                           .Include(x => x.ProductTags)
                           .Include(x => x.ProductCategories)
                           .ThenInclude(x => x.Category)
                           .ThenInclude(x => x.Children)
                           .FirstOrDefaultAsync(x => x.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }
            ViewBag.Brands = await _db.Brands.ToListAsync();
            ViewBag.Tags = await _db.Tags.ToListAsync();
            ViewBag.MainCategories = await _db.Categories.Where(c => c.IsMain).ToListAsync();
            ViewBag.ChildCategories = dbProduct.ProductCategories.FirstOrDefault().Category.Children;

            List<ProductImage> productImages = new List<ProductImage>();
           
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
                string folder = Path.Combine(_env.WebRootPath, "assets", "images", "product");
                ProductImage productImage = new ProductImage
                {
                    Image = await photo.SaveImageAsync(folder),
                };

                productImages.Add(productImage);
            }


            dbProduct.ProductImages.AddRange(productImages)  ;
            dbProduct.BrandId = brandId;
            List<ProductTag> productTags = new List<ProductTag>();

            foreach (int tagId in tagsId)
            {
                ProductTag productTag = new ProductTag
                {
                    TagId = tagId,

                };
                productTags.Add(productTag);
            }
            dbProduct.ProductTags = productTags;

            List<ProductCategory> productCategories = new List<ProductCategory>();

            ProductCategory mainProductCategory = new ProductCategory
            {
                CategoryId = mainCatId,
            };
            productCategories.Add(mainProductCategory);

            if (childCatId != null)
            {

                ProductCategory childProductCategory = new ProductCategory
                {
                    CategoryId = (int)childCatId,
                };
                productCategories.Add(childProductCategory);


            }

            dbProduct.ProductCategories = productCategories;
            dbProduct.Name = product.Name;
            dbProduct.Price = product.Price;
            dbProduct.ProductDetail.Description = product.ProductDetail.Description;
            dbProduct.ProductDetail.ProductCode = product.ProductDetail.ProductCode;
            dbProduct.ProductDetail.Tax = product.ProductDetail.Tax;

            return RedirectToAction("Index");

        }
        #endregion

        #region Detail
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Product product = await _db.Products.FirstOrDefaultAsync(t => t.Id == id);
            if (product == null)
            {
                return BadRequest();
            }
            return View(product);
        }
        #endregion

        #region Activity
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbProduct = await _db.Products.FirstOrDefaultAsync(t => t.Id == id);
            if (dbProduct == null)
            {
                return BadRequest();
            }

            if (dbProduct.IsDeactive)
            {
                dbProduct.IsDeactive = false;
            }
            else
            {
                dbProduct.IsDeactive = true;
            }
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
        #region DeleteImages
        public async Task<IActionResult> DeleteImages(int proImageId)
        {
            ProductImage productImage = await _db.ProductImages
                .FirstOrDefaultAsync(x => x.Id == proImageId);
            string folder = Path.Combine(_env.WebRootPath, "assets", "images", "product");
            Extensions.DeleteFile(folder, productImage.Image);
            _db.ProductImages.Remove(productImage);
            _db.SaveChanges();

            return Ok();
        }
        #endregion
    }
}
