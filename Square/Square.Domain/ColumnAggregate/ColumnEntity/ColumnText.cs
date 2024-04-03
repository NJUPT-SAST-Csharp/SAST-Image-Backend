using Primitives.Rule;

namespace Square.Domain.ColumnAggregate.ColumnEntity
{
    public readonly record struct ColumnText
    {
        public const int MaxLength = 200;

        public readonly string? Value { get; }

        public ColumnText(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                Value = null;
                return;
            }

            value = value.Trim();

            if (value.Length > MaxLength)
            {
                throw new DomainObjectValidationException(
                    $"The topic column text length must be less than {MaxLength}."
                );
            }

            Value = value;
        }
    }
}
