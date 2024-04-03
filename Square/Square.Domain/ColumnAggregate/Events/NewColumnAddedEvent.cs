using Microsoft.AspNetCore.Http;
using Shared.Primitives.DomainEvent;
using Square.Domain.ColumnAggregate.ColumnEntity;
using Square.Domain.ColumnAggregate.Commands.AddColumn;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.ColumnAggregate.Events
{
    public sealed class NewColumnAddedEvent(ColumnId id, AddColumnCommand command) : IDomainEvent
    {
        public ColumnId Id { get; } = id;

        public UserId AuthorId { get; } = command.Requester.Id;

        public TopicId TopicId { get; } = command.TopicId;

        public ColumnText ColumnText { get; } = command.Text;

        public IFormFileCollection Images { get; } = command.Images;
    }
}
