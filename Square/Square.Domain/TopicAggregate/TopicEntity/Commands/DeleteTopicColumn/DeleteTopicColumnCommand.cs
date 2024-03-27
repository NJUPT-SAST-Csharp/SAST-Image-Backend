using System.Security.Claims;
using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.TopicEntity.Commands.DeleteTopicColumn
{
    public sealed class DeleteTopicColumnCommand(long topicId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
