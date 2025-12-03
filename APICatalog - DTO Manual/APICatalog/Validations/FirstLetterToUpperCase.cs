using System.ComponentModel.DataAnnotations;

namespace APICatalog.Validations
{
    public class FirstLetterToUpperCase : ValidationAttribute
    {

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value == null|| string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var firstLetter = value.ToString()[0].ToString();
            if(firstLetter != firstLetter.ToUpper())
            {
                return new ValidationResult("The first name letter should start with upper");
            }

            return ValidationResult.Success;
        }
    }
}
