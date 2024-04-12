using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.ImageEntity
{
    public readonly struct ImageTitle
    {
        public const int MinLength = 1;
        public const int MaxLength = 12;

        public readonly string Value { get; } = string.Empty;

        public ImageTitle(string value)
        {
            if (
                string.IsNullOrWhiteSpace(value)
                || value.Length < MinLength
                || value.Length > MaxLength
            )
            {
                throw new DomainObjectValidationException(
                    $"Image title length must be between {MinLength} and {MaxLength}"
                );
            }

            Value = value;
        }
    }
}
