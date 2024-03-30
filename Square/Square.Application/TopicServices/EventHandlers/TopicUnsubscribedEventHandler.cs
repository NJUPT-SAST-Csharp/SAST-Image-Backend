using Exceptions.Exceptions;
using Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Events;

namespace Square.Application.TopicServices.EventHandlers
{
    internal sealed class TopicUnsubscribedEventHandler(ITopicQueryRepository repository)
        : IDomainEventHandler<TopicUnsubscribedEvent>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task Handle(
            TopicUnsubscribedEvent notification,
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

            topic.Unsubscribe(notification);
        }
    }
}
