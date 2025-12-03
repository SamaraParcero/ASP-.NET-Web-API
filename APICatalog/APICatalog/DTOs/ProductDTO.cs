using APICatalog.Models;
using System.ComponentModel.DataAnnotations;

namespace APICatalog.DTOs
{
    public class ProductDTO
    {
       
        public int ProductId { get; set; }
        [Required(ErrorMessage = " Name required")]
        [StringLength(80, ErrorMessage = " Name should have between 5 and 20 characters", MinimumLength = 5)]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(300)]
        public string Description { get; set; } = default!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        [StringLength(300)]
        public string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }
       
    }
}
