namespace AllUp3.Models
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeactive { get; set; }

        public List<Product> Products { get; set; }

    }
}
