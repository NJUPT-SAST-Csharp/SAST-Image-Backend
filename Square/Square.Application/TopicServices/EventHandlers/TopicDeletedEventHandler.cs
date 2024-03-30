using Primitives.DomainEvent;
using Square.Domain.TopicAggregate.Events;

namespace Square.Application.TopicServices.EventHandlers
{
    internal sealed class TopicDeletedEventHandler(ITopicQueryRepository repository)
        : IDomainEventHandler<TopicDeletedEvent>
    {
        private readonly ITopicQueryRepository _repository = repository;

        public Task Handle(TopicDeletedEvent notification, CancellationToken cancellationToken)
        {
            return _repository.DeleteTopicAsync(notification.TopicId).WaitAsync(cancellationToken);
        }
    }
}
