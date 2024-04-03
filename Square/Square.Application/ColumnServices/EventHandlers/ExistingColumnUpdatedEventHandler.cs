using Exceptions.Exceptions;
using Primitives.DomainEvent;
using Square.Application.ColumnServices.Models;
using Square.Application.TopicServices;
using Square.Domain.ColumnAggregate.Events;

namespace Square.Application.ColumnServices.EventHandlers
{
    internal sealed class ExistingColumnUpdatedEventHandler(
        IColumnQueryRepository repository,
        IColumnImageStorage storage
    ) : IDomainEventHandler<ExistingColumnUpdatedEvent>
    {
        private readonly IColumnImageStorage _storage = storage;
        private readonly IColumnQueryRepository _repository = repository;

        public async Task Handle(
            ExistingColumnUpdatedEvent notification,
            CancellationToken cancellationToken
        )
        {
            var column = await _repository
                .GetColumnAsync(notification.ColumnId)
                .WaitAsync(cancellationToken);

            if (column is null)
            {
                throw new DbNotFoundException(
                    nameof(ColumnModel),
                    notification.ColumnId.ToString()
                );
            }

            await column.UpdateAsync(notification, _storage);
        }
    }
}
