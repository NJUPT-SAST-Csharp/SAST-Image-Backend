using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate
{
    public interface IColumn : IDomainEventContainer
    {
        public ColumnId Id { get; }
        public void Like(UserId userId);
        public void Unlike(UserId userId);
        public bool IsManagedBy(in RequesterInfo user);
    }
}
