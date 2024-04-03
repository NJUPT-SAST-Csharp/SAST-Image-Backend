using Primitives.DomainEvent;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Square.Domain.ColumnAggregate.Events;

namespace Square.Application.ColumnServices.EventHandlers
{
    internal sealed class NewColumnAddEventHandler(
        IColumnQueryRepository repository,
        IColumnImageStorage storage
    ) : IDomainEventHandler<NewColumnAddedEvent>
    {
        private readonly IColumnImageStorage _storage = storage;
        private readonly IColumnQueryRepository _repository = repository;

        public async Task Handle(
            NewColumnAddedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var column = await ColumnModel
                .CreateNewColumnAsync(notification, _storage)
                .WaitAsync(cancellationToken);

            await _repository.AddColumnAsync(column);
        }
    }
}
