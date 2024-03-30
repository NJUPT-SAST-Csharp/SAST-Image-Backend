using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Commands.UnlikeColumn
{
    public sealed class UnlikeColumnCommand(long columnId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public ColumnId ColumnId { get; } = new(columnId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
