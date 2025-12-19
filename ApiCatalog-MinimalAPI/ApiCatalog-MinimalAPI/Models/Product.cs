using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ApiCatalog_MinimalAPI.Models
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

        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
