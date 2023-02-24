using AllUp3.Models;
using Microsoft.EntityFrameworkCore;

namespace AllUp3.DAL
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }

        public DbSet<Category > Categories { get; set; }
        public DbSet<Tag > Tags { get; set; }
        public DbSet<Brand > Brands { get; set; }
        public DbSet<Product > Products { get; set; }
        public DbSet<ProductDetail > ProductDetails { get; set; }
        public DbSet<ProductTag > ProductTags { get; set; }
        public DbSet<ProductCategory > ProductCategories { get; set; }
        public DbSet<ProductImage > ProductImages { get; set; }
    }
}
