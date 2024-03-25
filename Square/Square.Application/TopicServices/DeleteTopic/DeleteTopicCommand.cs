using System.Security.Claims;
using Primitives.Command;
using Square.Application.SeedWorks;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.DeleteTopic
{
    public sealed class DeleteTopicCommand(long topicId, ClaimsPrincipal user) : ICommandRequest
    {
        public TopicId TopicId { get; } = new(topicId);

        public RequesterInfo Requester { get; } = new(user);
    }
}
