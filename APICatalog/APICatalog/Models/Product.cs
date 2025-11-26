using APICatalog.Validations;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalog.Models
{
    [Table("Products")]
    public class Product : IValidatableObject
    {
        [Key]
        public int ProductId { get; set; }
        [Required(ErrorMessage = " Name required")]
        [StringLength(80, ErrorMessage = " Name should have between 5 and 20 characters", MinimumLength =5)]
        //[FirstLetterToUpperCase]
        public string Name { get; set; } = default!;
        [Required]
        [StringLength(300)]
        public string Description { get; set; } = default!;
        [Required]
        [Range(1,10000, ErrorMessage ="Price should be beetween {1} and {2}")]
        public decimal Price { get; set; }
        public float Stock { get; set; }
        public DateTime RegistrationDate { get; set; }
        [Required]
        [StringLength(300, MinimumLength =10)]
        public string ImageUrl { get; set; } = default!;
        public int CategoryId { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }

        //Recurso de validação pronto e não escalável para outros modelos
       public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Name))
            {
                var firstLetter = this.Name.ToString()[0].ToString();
                if (firstLetter != firstLetter.ToUpper())
                {
                    //yield é um iterador
                    yield return new
                        ValidationResult("The first name letter must be upper",
                        new[]
                        {nameof(this.Name)}
                        );
                }
            }

            if( this.Stock <= 0)
            {
                yield return new
                       ValidationResult("The stock must be greater than zero ",
                       new[]
                       {nameof(this.Stock)}
                       );
            }
        }






    }
}
