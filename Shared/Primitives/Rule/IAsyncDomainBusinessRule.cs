namespace Primitives.Rule
{
    public interface IAsyncDomainBusinessRule
    {
        public Task<bool> IsBrokenAsync();
        public string Message { get; }
    }
}
