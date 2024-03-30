using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class ColumnUnlikedEvent(ColumnId columnId, UserId userId) : IDomainEvent
    {
        public ColumnId ColumnId { get; } = columnId;
        public UserId UserId { get; } = userId;
    }
}
