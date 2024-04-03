using Primitives.DomainEvent;
using Square.Application.TopicServices;
using Square.Domain.ColumnAggregate.Events;

namespace Square.Application.ColumnServices.EventHandlers
{
    internal sealed class ColumnDeletedEventHandler(
        IColumnQueryRepository repository,
        IColumnImageStorage storage
    ) : IDomainEventHandler<ColumnDeletedEvent>
    {
        private readonly IColumnImageStorage _storage = storage;
        private readonly IColumnQueryRepository _repository = repository;

        public Task Handle(ColumnDeletedEvent notification, CancellationToken cancellationToken)
        {
            return _repository
                .DeleteColumnAsync(notification.ColumnId)
                .WaitAsync(cancellationToken);
        }
    }
}
