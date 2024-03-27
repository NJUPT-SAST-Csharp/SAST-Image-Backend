using System.Security.Claims;
using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.TopicEntity.Commands.UnsubscribeTopic
{
    public sealed class UnsubscribeTopicCommand(long topicId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
