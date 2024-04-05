namespace Square.Application.TopicServices.Queries.GetTopic
{
    public sealed class TopicDetailedDto
    {
        public string Title { get; private init; }
        public long TopicId { get; private init; }
        public string Description { get; private init; }
        public long AuthorId { get; private init; }
        public int CategoryId { get; private init; }

        public DateTime PublishedAt { get; private init; }
        public DateTime UpdatedAt { get; private init; }

        public int Columns { get; private init; }
        public int Subscribes { get; private init; }
        public bool IsSubscribed { get; private init; }

        public static TopicDetailedDto MapFrom(TopicModel model)
        {
            return new TopicDetailedDto()
            {
                Title = model.Title.Value,
                TopicId = model.Id.Value,
                Description = model.Description.Value,
                AuthorId = model.AuthorId.Value,
                CategoryId = model.CategoryId.Value,
                PublishedAt = model.PublishedAt,
                UpdatedAt = model.UpdatedAt,
                Columns = model.Columns.Count,
                Subscribes = model.Subscribes.Count,
                IsSubscribed = false
                // TODO: Implement IsSubscribed
            };
        }
    }
}
