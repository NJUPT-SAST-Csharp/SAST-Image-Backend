namespace Square.Application.TopicServices.Queries.GetTopics
{
    public sealed class TopicDto
    {
        public long TopicId { get; private init; }

        public long AuthorId { get; private init; }

        public string Title { get; private init; }

        public static TopicDto MapFrom(TopicModel topic) =>
            new()
            {
                TopicId = topic.Id.Value,
                AuthorId = topic.AuthorId.Value,
                Title = topic.Title.Value
            };
    }
}
