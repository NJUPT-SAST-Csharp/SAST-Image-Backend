namespace Primitives.Rules
{
    public sealed class DomainBusinessRuleInvalidException(IDomainBusinessRule brokenRule)
        : Exception(brokenRule.Message)
    {
        public IDomainBusinessRule BrokenRule { get; } = brokenRule;

        public string Details { get; } = brokenRule.Message;

        public override string ToString()
        {
            return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
        }
    }
}
