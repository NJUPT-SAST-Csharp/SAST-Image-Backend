namespace Primitives.Rules
{
    public interface IDomainBusinessRule
    {
        bool IsBroken { get; }

        string Message { get; }
    }
}
