using Primitives.Rule;

namespace Square.Domain.TopicAggregate.TopicEntity
{
    public readonly record struct TopicTitle
    {
        public const int MinLength = 1;
        public const int MaxLength = 20;

        public readonly string Value { get; }

        public TopicTitle(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new DomainObjectValidationException(
                    $"The topic title length must be between {MinLength} and {MaxLength}."
                );
            }

            value = value.Trim();

            if (value.Length < MinLength || value.Length > MaxLength)
            {
                throw new DomainObjectValidationException(
                    $"The topic title length must be between {MinLength} and {MaxLength}."
                );
            }

            Value = value;
        }

        public override readonly string ToString()
        {
            return Value;
        }
    }
}
