using System.Security.Claims;
using Primitives.Command;
using Square.Application.SeedWorks;
using Square.Domain.TopicAggregate.TopicEntity;

namespace Square.Application.TopicServices.UpdateTopicInfo
{
    public sealed class UpdateTopicInfoCommand(
        long topicId,
        string title,
        string description,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public TopicId TopicId { get; } = new(topicId);

        public string Title { get; } = title;

        public string Description { get; } = description;

        public RequesterInfo Requester { get; } = new(user);
    }
}
