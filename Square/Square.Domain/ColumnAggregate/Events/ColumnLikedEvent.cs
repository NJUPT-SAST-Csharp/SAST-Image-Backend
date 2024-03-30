using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class ColumnLikedEvent(ColumnId columnId, UserId likedBy) : IDomainEvent
    {
        public UserId LikedBy { get; } = likedBy;
        public ColumnId ColumnId { get; } = columnId;
    }
}
