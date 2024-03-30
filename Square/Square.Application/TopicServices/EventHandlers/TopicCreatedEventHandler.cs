using Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Events;

namespace Square.Application.TopicServices.EventHandlers
{
    internal sealed class TopicCreatedEventHandler(ITopicQueryRepository repository)
        : IDomainEventHandler<TopicCreatedEvent>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public async Task Handle(
            TopicCreatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var topic = TopicModel.CreateNewTopic(notification);

            await _repository.AddTopicAsync(topic).WaitAsync(cancellationToken);
        }
    }
}
