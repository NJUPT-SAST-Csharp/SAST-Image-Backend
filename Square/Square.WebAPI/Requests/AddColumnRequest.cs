namespace Square.WebAPI.Requests
{
    public sealed class AddColumnRequest
    {
        public string? Text { get; init; }
        public IFormFileCollection Images { get; init; }
    }
}
