namespace APICatalog.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public float Stock { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
