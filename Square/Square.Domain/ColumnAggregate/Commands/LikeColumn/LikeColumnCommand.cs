using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.ColumnAggregate.ColumnEntity;

namespace Square.Domain.ColumnAggregate.Commands.LikeColumn
{
    public sealed class LikeColumnCommand(long columnId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public ColumnId ColumnId { get; } = new(columnId);

        public RequesterInfo Requester { get; } = new(user);
    }
}
