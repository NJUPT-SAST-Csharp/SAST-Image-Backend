using Primitives.Rule;

namespace Square.Domain.TopicAggregate.TopicEntity
{
    public readonly struct TopicColumnText
    {
        public const int MaxLength = 200;

        public readonly string? Value { get; }

        public TopicColumnText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = null;
                return;
            }

            value = value.Trim();

            if (value?.Length > MaxLength)
            {
                throw new DomainObjectValidationException(
                    $"The topic column text length must be less than {MaxLength}."
                );
            }

            Value = value;
        }

        public override readonly string? ToString()
        {
            return Value;
        }
    }
}
