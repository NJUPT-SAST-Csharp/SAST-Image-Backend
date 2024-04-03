using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class ColumnDeletedEvent(ColumnId id) : IDomainEvent
    {
        public ColumnId ColumnId { get; } = id;
    }
}
