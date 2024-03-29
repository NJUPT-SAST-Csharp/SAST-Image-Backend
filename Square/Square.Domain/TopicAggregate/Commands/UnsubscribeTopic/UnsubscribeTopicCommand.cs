using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.UnsubscribeTopic
{
    public sealed class UnsubscribeTopicCommand(long topicId, ClaimsPrincipal user)
        : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
