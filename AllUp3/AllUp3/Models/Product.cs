using System.ComponentModel.DataAnnotations.Schema;

namespace AllUp3.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public bool IsDeactive { get; set; }

        public ProductDetail ProductDetail { get; set; }

        public List<ProductTag> ProductTags{ get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public List<ProductImage> ProductImages { get; set; }
        [NotMapped]
        public List<IFormFile> Photos { get; set; }

        public Brand Brand { get; set; }
        public int BrandId { get; set; }




    }
}
