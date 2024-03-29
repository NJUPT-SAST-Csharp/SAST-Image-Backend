using System.Security.Claims;
using FoxResult;
using Primitives.Command;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Domain.TopicAggregate.Commands.UpdateTopicInfo
{
    public sealed class UpdateTopicInfoCommand(
        long topicId,
        string title,
        string description,
        ClaimsPrincipal user
    ) : ICommandRequest<Result>
    {
        public TopicId TopicId { get; } = new(topicId);

        public TopicTitle Title { get; } = new(title);

        public TopicDescription Description { get; } = new(description);

        public RequesterInfo Requester { get; } = new(user);
    }
}
