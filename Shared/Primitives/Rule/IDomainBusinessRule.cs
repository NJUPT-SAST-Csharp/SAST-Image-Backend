namespace Primitives.Rule
{
    public interface IDomainBusinessRule
    {
        bool IsBroken { get; }

        string Message { get; }
    }
}
