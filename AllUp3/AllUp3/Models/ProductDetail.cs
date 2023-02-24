using System.ComponentModel.DataAnnotations.Schema;

namespace AllUp3.Models
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public double Tax { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public bool HasStock { get; set; }
        public Product Product { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }


    }
}
