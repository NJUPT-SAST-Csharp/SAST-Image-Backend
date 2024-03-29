using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Commands.DeleteColumn
{
    public sealed class DeleteColumnCommand(long topicId, long columnAuthorId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public ColumnId ColumnId { get; } = new(new(topicId), new(columnAuthorId));
        public RequesterInfo Requester { get; } = new(user);
    }
}
