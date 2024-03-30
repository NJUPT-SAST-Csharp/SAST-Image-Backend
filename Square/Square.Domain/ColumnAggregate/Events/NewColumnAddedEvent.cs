using Microsoft.AspNetCore.Http;
using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.ColumnAggregate.Commands.AddColumn;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class NewColumnAddedEvent(ColumnId id, AddColumnCommand command) : IDomainEvent
    {
        public ColumnId Id { get; } = id;

        public ColumnText ColumnText { get; } = command.Text;

        public IFormFileCollection Images { get; } = command.Images;
    }
}
