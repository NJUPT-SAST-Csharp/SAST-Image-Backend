using Microsoft.AspNetCore.Http;
using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.ColumnAggregate.Commands.AddColumn;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class ExistingColumnUpdatedEvent(ColumnId columnId, AddColumnCommand command)
        : IDomainEvent
    {
        public ColumnId ColumnId { get; } = columnId;

        public ColumnText ColumnText { get; } = command.Text;

        public IFormFileCollection Images { get; } = command.Images;
    }
}
