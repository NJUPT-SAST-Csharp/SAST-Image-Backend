using Exceptions.Exceptions;
using Primitives.DomainEvent;
using Square.Application.ColumnServices.Models;
using Square.Domain.ColumnAggregate.Events;

namespace Square.Application.ColumnServices.EventHandlers
{
    internal sealed class ColumnLikedEventHandler(IColumnQueryRepository repository)
        : IDomainEventHandler<ColumnLikedEvent>
    {
        private readonly IColumnQueryRepository _repository = repository;

        public async Task Handle(ColumnLikedEvent notification, CancellationToken cancellationToken)
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

            column.Like(notification);
        }
    }
}
