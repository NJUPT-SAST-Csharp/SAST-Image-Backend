using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    public readonly struct AlbumTitle
    {
        public const int MinLength = 2;
        public const int MaxLength = 10;

        public readonly string Value { get; } = string.Empty;

        public AlbumTitle(string value)
        {
            if (
                string.IsNullOrWhiteSpace(value)
                || value.Length < MinLength
                || value.Length > MaxLength
            )
            {
                throw new DomainObjectValidationException(
                    $"Album title length must be between {MinLength} and {MaxLength}"
                );
            }

            Value = value;
        }

        public static implicit operator AlbumTitle(string title)
        {
            return new(title);
        }
    }
}
