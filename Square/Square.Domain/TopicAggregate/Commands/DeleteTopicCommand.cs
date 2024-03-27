using System.Security.Claims;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands
{
    public sealed class DeleteTopicCommand(long topicId, ClaimsPrincipal user)
    {
        public TopicId TopicId { get; } = new(topicId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
