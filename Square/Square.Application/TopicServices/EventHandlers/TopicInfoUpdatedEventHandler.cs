using Exceptions.Exceptions;
using Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Events;

namespace Square.Application.TopicServices.EventHandlers
{
    internal sealed class TopicInfoUpdatedEventHandler(ITopicQueryRepository repository)
        : IDomainEventHandler<TopicInfoUpdatedEvent>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task Handle(
            TopicInfoUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var topic = await _repository
                .GetTopicAsync(notification.TopicId)
                .WaitAsync(cancellationToken);

            if (topic is null)
            {
                throw new DbNotFoundException(nameof(TopicModel), notification.TopicId.ToString());
            }

            topic.UpdateTopicInfo(notification);
        }
    }
}
