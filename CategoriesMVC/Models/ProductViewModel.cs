using System.ComponentModel.DataAnnotations;

namespace CategoriesMVC.Models
{
    public class ProductViewModel
    {
        public int ProdutoId { get; set; }
        [Required(ErrorMessage = "Product's name is required")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "Product's description is required")]
        public string? Descricao { get; set; }
        [Required(ErrorMessage = "Product's price is required")]
        public decimal Preco { get; set; }
        [Required(ErrorMessage = "Product's Image's Path is required")]
        [Display(Name = "Image's path")]
        public string? ImagemUrl { get; set; }

        [Display(Name = "Category")]
        public int CategoriaId { get; set; }
    }
}
