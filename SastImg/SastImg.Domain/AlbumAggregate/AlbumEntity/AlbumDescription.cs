using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    public readonly struct AlbumDescription
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public readonly string Value { get; } = string.Empty;

        public AlbumDescription(string value)
        {
            if (
                string.IsNullOrWhiteSpace(value)
                || value.Length < MinLength
                || value.Length > MaxLength
            )
            {
                throw new DomainObjectValidationException(
                    $"Album description length must be between {MinLength} and {MaxLength}"
                );
            }

            Value = value;
        }

        public static implicit operator AlbumDescription(string description)
        {
            return new(description);
        }
    }
}
