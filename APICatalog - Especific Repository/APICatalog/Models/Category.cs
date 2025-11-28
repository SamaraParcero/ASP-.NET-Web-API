using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models
{
    [Table("Categorys")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(80)]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; } = default!;
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }

        public Category()
        {
            Products = new Collection<Product>();
        }
    }
}
