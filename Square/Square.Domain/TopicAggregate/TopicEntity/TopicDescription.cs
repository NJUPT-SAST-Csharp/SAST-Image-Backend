using Primitives.Rule;

namespace Square.Domain.TopicAggregate.TopicEntity
{
    public readonly struct TopicDescription
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public readonly string Value { get; }

        public TopicDescription(string value)
        {
            if (
                string.IsNullOrWhiteSpace(value)
                || value.Length < MinLength
                || value.Length > MaxLength
            )
            {
                throw new DomainObjectValidationException(
                    $"The topic description length must be between {MinLength} and {MaxLength}."
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
