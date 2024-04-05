namespace Square.WebAPI.Requests
{
    public sealed class CreateTopicRequest
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public int CategoryId { get; init; }
    }
}
