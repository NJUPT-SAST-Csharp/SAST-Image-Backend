using System.Runtime.CompilerServices;

namespace Primitives.Rule
{
    public sealed class DomainBusinessRuleInvalidException : Exception
    {
        public DomainBusinessRuleInvalidException(in IDomainBusinessRule rule, string field)
            : base(rule.Message)
        {
            FieldName = field;
        }

        public DomainBusinessRuleInvalidException(in IAsyncDomainBusinessRule rule, string field)
            : base(rule.Message)
        {
            FieldName = field;
        }

        public DomainBusinessRuleInvalidException(
            string message,
            [CallerMemberName] string field = null!
        )
            : base(message)
        {
            FieldName = field;
        }

        public string FieldName { get; }

        public override string ToString()
        {
            return $"{FieldName}: {Message}";
        }
    }
}
