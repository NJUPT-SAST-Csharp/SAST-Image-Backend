using Square.Application.ColumnServices.Models;
using Square.Domain;
using Square.Domain.TopicAggregate.Events;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices
{
    public sealed class TopicModel
    {
        private TopicModel() { }

        public TopicId Id { get; private init; }
        public UserId AuthorId { get; private init; }
        public TopicTitle Title { get; private set; }
        public TopicDescription Description { get; private set; }
        public DateTime PublishedAt { get; private init; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public List<TopicSubscribe> Subscribes { get; private init; }

        public List<ColumnModel> Columns { get; private init; }

        public static TopicModel CreateNewTopic(TopicCreatedEvent e)
        {
            return new TopicModel()
            {
                Id = e.Id,
                AuthorId = e.Requester.Id,
                Description = e.Description,
                Title = e.Title
            };
        }

        public void UpdateTopicInfo(TopicInfoUpdatedEvent e)
        {
            Title = e.Title;
            Description = e.Description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Subscribe(TopicSubscribedEvent e)
        {
            Subscribes.Add(new TopicSubscribe(e.SubscriberId, Id));
        }

        public void Unsubscribe(TopicUnsubscribedEvent e)
        {
            Subscribes.RemoveAll(s => s.UserId == e.UserId);
        }
    }
}
