using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
public sealed class FileSizeAttribute : ValidationAttribute
{
    private const int multiple = 1024 * 1024; // 1 MB in bytes

    private long min = ImageFile.DefaultMinSize;
    private long max = ImageFile.DefaultMaxSize;

    public int MinMB
    {
        set => min = multiple * value;
    }
    public long MaxMB
    {
        set => max = multiple * value;
    }

    private static readonly ValidationResult InvalidType = new("Invalid file type.");
    private static readonly ValidationResult? Success = ValidationResult.Success;
    private static readonly Func<long, long, ValidationResult> InvalidSize = (min, max) =>
        new($"File size is must between {min} and {max} Bytes.");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null)
            return Success;

        var invalidSize = InvalidSize(min, max);

        return value switch
        {
            IImageFile and { Length: var length } when length.Beyond(min, max) => invalidSize,

            IFormFile and { Length: var length } when length.Beyond(min, max) => invalidSize,

            IFormFileCollection collection
                when collection.Any(file => file.Length.Beyond(min, max)) => invalidSize,
            IImageFileCollection collection
                when collection.Any(file => file.Length.Beyond(min, max)) => invalidSize,

            not (IImageFile or IImageFileCollection or IFormFile or IFormFileCollection) =>
                InvalidType,

            _ => Success,
        };
    }
}

file static class IntExtensions
{
    public static bool Between(this long value, long min, long max)
    {
        return value >= min && value <= max;
    }

    public static bool Beyond(this long value, long min, long max)
    {
        return value < min || value > max;
    }
}
