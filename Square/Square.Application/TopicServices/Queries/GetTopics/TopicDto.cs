namespace Square.Application.TopicServices.Queries.GetTopics
{
    public sealed class TopicDto
    {
        public long TopicId { get; private init; }

        public long AuthorId { get; private init; }

        public string Title { get; private init; }

        public Uri[] Previews { get; private init; }

        public DateTime UpdatedAt { get; private init; }

        public static TopicDto MapFrom(TopicModel topic)
        {
            var previews = topic
                .Columns.SelectMany(c => c.Images)
                .Select(p => p.ThumbnailUrl)
                .ToArray();

            Random.Shared.Shuffle(previews);

            previews = previews.Take(10).ToArray();

            return new()
            {
                TopicId = topic.Id.Value,
                AuthorId = topic.AuthorId.Value,
                Title = topic.Title.Value,
                UpdatedAt = topic.UpdatedAt,
                Previews = previews,
            };
        }
    }
}
