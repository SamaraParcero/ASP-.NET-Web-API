using System.ComponentModel.DataAnnotations;

namespace CategoriesMVC.Models
{
    public class CategoryViewModel
    {
        [Key]
        public int CategoriaId { get; set; }
        [Required]
        [StringLength(80)]
        public string Nome { get; set; } = default!;
        [Required]
        [StringLength(300)]
        public string ImagemUrl { get; set; } = default!;
    }
}
