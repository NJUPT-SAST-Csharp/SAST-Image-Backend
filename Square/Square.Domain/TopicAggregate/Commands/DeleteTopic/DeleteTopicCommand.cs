using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.DeleteTopic
{
    public sealed class DeleteTopicCommand(long topicId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
