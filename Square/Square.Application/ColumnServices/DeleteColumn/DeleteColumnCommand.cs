using System.Security.Claims;
using Primitives.Command;
using Square.Application.SeedWorks;
using Square.Domain.TopicAggregate.ColumnEntity;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.ColumnServices.DeleteColumn
{
    public sealed class DeleteColumnCommand(long topicId, long columnId, ClaimsPrincipal user)
        : ICommandRequest
    {
        public TopicId TopicId { get; } = new(topicId);
        public ColumnId ColumnId { get; } = new(columnId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
