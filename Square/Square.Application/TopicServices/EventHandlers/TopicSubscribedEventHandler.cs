using Exceptions.Exceptions;
using Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Events;

namespace Square.Application.TopicServices.EventHandlers
{
    internal sealed class TopicSubscribedEventHandler(ITopicQueryRepository repository)
        : IDomainEventHandler<TopicSubscribedEvent>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task Handle(
            TopicSubscribedEvent notification,
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

            topic.Subscribe(notification);
        }
    }
}
