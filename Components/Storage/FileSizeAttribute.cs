using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Storage;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, AllowMultiple = false)]
public sealed class FileSizeAttribute : ValidationAttribute
{
    public int MinMB { get; init; } = 0;
    public int MaxMB { get; init; } = ImageFile.MaxSize;

    private static readonly ValidationResult InvalidType = new("Invalid file type.");
    private static readonly ValidationResult? Success = ValidationResult.Success;
    private static readonly Func<int, int, ValidationResult> InvalidSize = (int min, int max) =>
        new($"File size is must between {min} and {max} MBs.");

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        int min = MinMB * 1024 * 1024;
        int max = MaxMB * 1024 * 1024;

        if (value is null)
            return Success;

        var invalidSize = InvalidSize(MinMB, MaxMB);

        return value switch
        {
            IImageFile and { Length: var length } when length.Beyond(min, max) => invalidSize,

            IFormFile and { Length: var length } when length.Beyond(min, max) => invalidSize,

            IFormFileCollection collection
                when collection.Any(file => file.Length.Beyond(min, max)) => invalidSize,
            IImageFileCollection collection
                when collection.Any(file => file.Length.Beyond(min, max)) => invalidSize,

            not IImageFile
            and not IImageFileCollection
            and not IFormFile
            and not IFormFileCollection => InvalidType,

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
