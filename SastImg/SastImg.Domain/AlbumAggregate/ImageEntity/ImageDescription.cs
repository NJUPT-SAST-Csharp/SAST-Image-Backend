using Primitives.Rule;

namespace SastImg.Domain.AlbumAggregate.ImageEntity
{
    public readonly struct ImageDescription
    {
        public const int MaxLength = 100;

        public readonly string Value { get; } = string.Empty;

        public ImageDescription(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (value.Length > MaxLength)
            {
                throw new DomainObjectValidationException(
                    $"Image description length must be less than {MaxLength}"
                );
            }

            Value = value;
        }
    }
}
