using System.Security.Claims;
using FoxResult;
using Primitives.Command;

namespace Square.Domain.TopicAggregate.TopicEntity.Commands.SubscribeTopic
{
    public sealed class SubscribeTopicCommand(long topicId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
