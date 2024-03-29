using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Commands.LikeColumn
{
    public sealed class ColumnLikedDomainEvent(ColumnId columnId, UserId likedBy) : IDomainEvent
    {
        public UserId LikedBy { get; } = likedBy;
        public ColumnId ColumnId { get; } = columnId;

        public DateTime OccuredOn { get; } = DateTime.UtcNow;
    }
}
