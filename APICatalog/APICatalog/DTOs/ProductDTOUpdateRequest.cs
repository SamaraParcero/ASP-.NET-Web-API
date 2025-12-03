using System.ComponentModel.DataAnnotations;

namespace APICatalog.DTOs
{
    public class ProductDTOUpdateRequest
    {
        [Range(1, 99999, ErrorMessage = "Stock should be beetween {1} and {2}")]
        public float Stock { get; set; }
        public DateTime RegistrationDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(RegistrationDate <= DateTime.Now.Date)
            {
                yield return new ValidationResult("The date should be greater that actual date", new[] { nameof(this.RegistrationDate) });
            }
        }
    }
}
