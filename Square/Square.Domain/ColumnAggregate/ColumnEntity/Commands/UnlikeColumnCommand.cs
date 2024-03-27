using System.Security.Claims;
using FoxResult;
using Primitives.Command;

namespace Square.Domain.ColumnAggregate.ColumnEntity.Commands
{
    public sealed class UnlikeColumnCommand(long topicId, long columnAuthorId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public ColumnId ColumnId { get; } = new((new(topicId), new(columnAuthorId)));
        public RequesterInfo Requester { get; } = new(user);
    }
}
