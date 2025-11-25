using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(300)]
        public string Description { get; set; } = default!;
        public decimal Price { get; set; }
        public float Stock { get; set; }
        public DateTime RegistrationDate { get; set; }
        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
    }
}
