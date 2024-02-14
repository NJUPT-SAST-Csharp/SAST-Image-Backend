using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Utilities.Validators
{
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class FileValidator(int maxMB) : ValidationAttribute
    {
        private readonly int _maxMB = maxMB;

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext
        )
        {
            if (value is not IFormFile file)
            {
                return new ValidationResult("No file is provided.");
            }
            if (file.Length > _maxMB * 1024 * 1024)
            {
                return new ValidationResult("Too large file. Max " + _maxMB + " MBs.");
            }
            if (file.ContentType.Contains("image") == false)
            {
                return new ValidationResult("Not supported file type.");
            }

            return ValidationResult.Success;
        }
    }
}
