using System.ComponentModel.DataAnnotations;
using SkiaSharp;

namespace WebAPI.Utilities.Attributes;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
public sealed class FileValidator(int minMB, int maxMB) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return ValidationResult.Success;
        if (value is not IFormFile file)
            return new ValidationResult("No file is provided.");
        if (file.Length < minMB * 1024 * 1024)
            return new ValidationResult("Too small file. Min " + minMB + " MBs.");
        if (file.Length > maxMB * 1024 * 1024)
            return new ValidationResult("Too large file. Max " + maxMB + " MBs.");
        if (file.ContentType.Contains("image") == false)
            return new ValidationResult("Not supported file type.");
        using var stream = file.OpenReadStream();
        using SKFrontBufferedManagedStream skStream = new(
            stream,
            SKCodec.MinBufferedBytesNeeded,
            true
        );
        using var code = SKCodec.Create(skStream);
        if (code is null)
            return new ValidationResult("Not supported file type.");

        return ValidationResult.Success;
    }
}
